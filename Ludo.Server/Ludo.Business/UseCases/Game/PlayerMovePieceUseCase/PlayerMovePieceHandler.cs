using FluentValidation;
using Ludo.Domain.DTOs;
using Ludo.Domain.Entities;
using Ludo.Domain.Enums;
using Ludo.Domain.Interfaces;
using Ludo.MediatRPattern.Interfaces;

namespace Ludo.Business.UseCases.Game.PlayerMovePieceUseCase
{
    public class PlayerMovePieceHandler : IRequestHandler<PlayerMovePieceRequest, (List<PieceDto>, List<IPlayer>, string, List<IPlayer>)>
    {
        private readonly IGameService _gameService;
        private readonly IPieceService _pieceService;

        public PlayerMovePieceHandler(IGameService gameService, IPieceService pieceService)
        {
            _gameService = gameService ?? throw new ArgumentNullException(nameof(gameService));
            _pieceService = pieceService ?? throw new ArgumentNullException(nameof(pieceService));
        }

        //TODO: Fix problem when trying to take another piece if you both are on the same cell

        public Task<(List<PieceDto>, List<IPlayer>, string, List<IPlayer>)> Handle(PlayerMovePieceRequest request)
        {
            bool isGameFinished = false;
            bool playerShouldBeRemovedFromQueue = false;

            IGame game = _gameService.GetGameById(request.GameId);

            if (game == null)
            {
                throw new Exception("Game doesn't exists");
            }

            List<PieceDto> piecesMoved = new List<PieceDto>();

            int basePosition = _pieceService.GetBasePosition(request.Piece.Color);

            //Case when Piece is on final cell
            if (CheckIfPieceIsOnFinalCell((int)request.Piece.PreviousPosition))
            {
                MovePieceFromFinalCell(request, game, piecesMoved, basePosition);

                CheckIfPlayerShouldBeRemovedFromQueue(request, out isGameFinished, out playerShouldBeRemovedFromQueue, game);
            }
            else
            {
                int nextPosition;

                //Case when Piece is not on board and I first rolled 6 and I can move piece to home cell
                if (CheckIfICanPutMyFirstPieceOnHomeCell(request))
                {
                    //should put on the table
                    //but I will pass 6 cells

                    PutPieceOnHomeCell(request, game, piecesMoved, basePosition);
                }////////////////////////////////////////////////////////////////////
                else/* if (!(basePosition == request.Piece.PreviousPosition))*/
                {
                    //Case when I have at least one piece which is not on home cell

                    ICell currentCell = game.Board.Cells[(int)request.Piece.PreviousPosition];
                    Piece currentPiece;

                    currentPiece = currentCell.Pieces.FirstOrDefault();

                    if (currentCell.Pieces.Count() != 1)
                    {
                        currentPiece = currentCell.Pieces.Where(p => p.Color == request.Piece.Color).FirstOrDefault();
                    }

                    nextPosition = ((int)request.Piece.PreviousPosition + request.DiceNumber);

                    int specialCellUpperBound = (basePosition - 2);

                    if (basePosition == 0)
                    {
                        specialCellUpperBound = 52 - 2;
                    }

                    int specialCellLowerBound = specialCellUpperBound - 6;

                    //Case when I try to go to a final cell
                    if (CheckIfICanMovePieceOnFinalCell(request, nextPosition, specialCellUpperBound, specialCellLowerBound))
                    {
                        MovePieceOnFinalCell(request, game, piecesMoved, nextPosition, currentCell, currentPiece, specialCellUpperBound);

                        CheckIfPlayerShouldBeRemovedFromQueue(request, out isGameFinished, out playerShouldBeRemovedFromQueue, game);
                    }
                    else
                    {
                        //Case when Piece moves normally and I try to replace a new piece

                        RemoveEnemyPiece(request, game, piecesMoved, nextPosition);
                        MovePiece(piecesMoved, game.Board.Cells, currentCell, currentPiece, nextPosition, (int)request.Piece.PreviousPosition);
                    }
                }
            }

            var playersWithouCaller = _gameService.GetPlayersWithoutCaller(game, request.ConnectionId);

            if (playerShouldBeRemovedFromQueue)
            {
                game.RollDiceOrder.RemoveFirst();
            }

            string nextDiceRoller = _gameService.GetNextDiceRoller(game);

            List<IPlayer> ranking = new List<IPlayer>();

            if (isGameFinished)
            {
                ranking = game.Ranking;
            }

            (List<PieceDto> pieces, List<IPlayer> players, string connectionId, List<IPlayer> ranking) result = (piecesMoved, playersWithouCaller, nextDiceRoller, ranking);

            return Task.FromResult(result);
        }

        private void CheckIfPlayerShouldBeRemovedFromQueue(PlayerMovePieceRequest request, out bool isGameFinished, out bool playerShouldBeRemovedFromQueue, IGame game)
        {
            IPlayer? player = game.Players.FirstOrDefault(p => p.ConnectionId == request.ConnectionId);
            playerShouldBeRemovedFromQueue = _gameService.CheckIfPlayerPicesAreOnTriangleCell(player);
            isGameFinished = _gameService.CheckIfGameIsFinished(game, player);
        }

        private void RemoveEnemyPiece(PlayerMovePieceRequest request, IGame game, List<PieceDto> piecesMoved, int nextPosition)
        {
            nextPosition %= 52;

            if (!(game.Board.Cells[nextPosition] is HomeCell))
            {
                var differentColorPieces = game.Board.Cells[nextPosition].Pieces.Where(p => p.Color != request.Piece.Color).ToList();

                foreach (var piece in differentColorPieces)
                {
                    SpawnPieces? playerSpawnPieces = game.Board.SpawnPositions.FirstOrDefault(p => p.Color == piece.Color);

                    int emptySpawnPieceIndex = playerSpawnPieces.Pieces.FindIndex(p => p == null);

                    playerSpawnPieces.Pieces[emptySpawnPieceIndex] = piece;

                    int spawnPosition = _pieceService.GetSpawnPosition(piece.Color, emptySpawnPieceIndex);

                    piecesMoved.Add(new PieceDto
                    {
                        Color = piece.Color,
                        NextPosition = spawnPosition,
                        PreviousPosition = nextPosition
                    });
                }

                game.Board.Cells[nextPosition].Pieces = game.Board.Cells[nextPosition].Pieces.Except(differentColorPieces).ToList();
            }
        }

        private void MovePiece(List<PieceDto> piecesMoved, List<ICell> boardCells, ICell currentCell, Piece currentPiece, int nextPosition, int previousPosition)
        {
            nextPosition %= 52;

            boardCells[nextPosition].Pieces.Add(currentPiece);

            currentCell.Pieces.Remove(currentPiece);

            AddNewPieceMoved(piecesMoved, currentPiece.Color, nextPosition, previousPosition);
        }

        //Returns true if player should be removed from queue
        private void MovePieceOnFinalCell
        (
            PlayerMovePieceRequest request, IGame game, List<PieceDto> piecesMoved,
            int nextPosition, ICell currentCell, Piece currentPiece, int specialCellUpperBound
        )
        {
            List<ICell> boardCells = game.Board.Cells;

            var specialCell = boardCells[specialCellUpperBound] as SpecialCell;

            if (nextPosition - specialCellUpperBound == 6)
            {
                IPlayer? player = game.Players.FirstOrDefault(p => p.ConnectionId == request.ConnectionId);

                currentCell.Pieces.Remove(currentPiece);
                player.Pieces.Remove(currentPiece);

                RemoveSpawnPosition(game, currentPiece);

                _gameService.PieceMovedOnWinningCell(game, player);

                int winningCellNextPosition = CreateWinningCellPosition(currentPiece.Color);
                AddNewPieceMoved(piecesMoved, currentPiece.Color, winningCellNextPosition, request.Piece.PreviousPosition.Value);
            }
            else
            {
                specialCell.FinalCells[nextPosition - specialCellUpperBound - 1].Pieces.Add(currentPiece);
                currentCell.Pieces.Remove(currentPiece);

                var specialCellNextPosition = CreateFinalCellPosition(currentPiece.Color, nextPosition - specialCellUpperBound - 1);

                AddNewPieceMoved(piecesMoved, currentPiece.Color, specialCellNextPosition, (int)request.Piece.PreviousPosition);
            }
        }

        private void RemoveSpawnPosition(IGame game, Piece currentPiece)
        {
            var spawnPosition = game.Board.SpawnPositions.FirstOrDefault(p => p.Color == currentPiece.Color);

            var emptySpawnPositionIndex = spawnPosition.Pieces.IndexOf(null);

            if (emptySpawnPositionIndex != -1)
            {
                spawnPosition.Pieces.RemoveAt(emptySpawnPositionIndex);
            }
        }

        private static bool CheckIfICanMovePieceOnFinalCell(PlayerMovePieceRequest request, int nextPosition, int specialCellUpperBound, int specialCellLowerBound)
        {
            //if(specialCellUpperBound == 50 && nextPosition>=0 && nextPosition<=4)
            //{
            //    nextPosition = specialCellUpperBound + nextPosition + 2;
            //}

            return request.Piece.PreviousPosition >= specialCellLowerBound &&
                                    request.Piece.PreviousPosition <= specialCellUpperBound &&
                                    nextPosition > specialCellUpperBound;
        }

        private void PutPieceOnHomeCell(PlayerMovePieceRequest request, IGame game, List<PieceDto> piecesMoved, int basePosition)
        {
            ColorType pieceColor = request.Piece.Color;

            SpawnPieces spawnPosition = game.Board.SpawnPositions.FirstOrDefault(p => p.Color == pieceColor);

            int pieceIndex = request.Piece.PreviousPosition.Value % 10;

            Piece currentPiece = spawnPosition.Pieces.FirstOrDefault(p => p != null);

            if (pieceIndex < spawnPosition.Pieces.Count)
            {
                currentPiece = spawnPosition.Pieces[pieceIndex % spawnPosition.Pieces.Count];
            }
            else
            {
                currentPiece = spawnPosition.Pieces.LastOrDefault(p => p != null);
            }

            int colorIndex = (int)pieceColor;

            game.Board.Cells[basePosition].Pieces.Add(currentPiece);

            pieceIndex = spawnPosition.Pieces.IndexOf(currentPiece);
            spawnPosition.Pieces[pieceIndex] = null;

            AddNewPieceMoved(piecesMoved, currentPiece.Color, basePosition, request.Piece.PreviousPosition.Value);
        }

        private bool CheckIfICanPutMyFirstPieceOnHomeCell(PlayerMovePieceRequest request)
        {
            return request.Piece.PreviousPosition >= 610 && request.Piece.PreviousPosition <= 943 && request.DiceNumber == 6;
        }

        private void MovePieceFromFinalCell(PlayerMovePieceRequest request, IGame game, List<PieceDto> piecesMoved, int basePosition)
        {
            List<ICell> boardCells = game.Board.Cells;

            int nextPosition = (int)(request.Piece.PreviousPosition + request.DiceNumber);

            int specialCellsPosition = basePosition - 2;

            if (basePosition == 0)
            {
                specialCellsPosition = 52 - 2;
            }

            SpecialCell specialCells = boardCells[specialCellsPosition] as SpecialCell;

            ICell specialCell = specialCells.FinalCells[(int)(request.Piece.PreviousPosition % 10)];

            Piece? currentPiece = specialCell.Pieces.Where(p => p.Color == request.Piece.Color).FirstOrDefault();

            if (nextPosition % 10 == 5)
            {
                //Piece is on triangle cell
                IPlayer? player = game.Players.FirstOrDefault(p => p.ConnectionId == request.ConnectionId);

                player.Pieces.Remove(currentPiece);
                specialCells.Pieces.Remove(currentPiece);

                RemoveSpawnPosition(game, currentPiece);

                _gameService.PieceMovedOnWinningCell(game, player);

                int winningCellNextPosition = CreateWinningCellPosition(currentPiece.Color);
                AddNewPieceMoved(piecesMoved, currentPiece.Color, winningCellNextPosition, (int)request.Piece.PreviousPosition);
            }
            else if (nextPosition % 10 < 5)
            {
                int finalCellNextPosition = CreateFinalCellPosition(currentPiece.Color, nextPosition % 10);
                specialCells.FinalCells[(int)request.Piece.PreviousPosition % 10].Pieces.Remove(currentPiece);
                specialCells.FinalCells[nextPosition % 10].Pieces.Add(currentPiece);
                AddNewPieceMoved(piecesMoved, currentPiece.Color, finalCellNextPosition, (int)request.Piece.PreviousPosition);
            }
        }

        private static bool CheckIfPieceIsOnFinalCell(int previousPosition)
        {
            return previousPosition >= 110 && previousPosition <= 444;
        }

        private int CreateWinningCellPosition(ColorType pieceColor)
        {
            string position = $"{(int)pieceColor}{(int)pieceColor}5";

            return int.Parse(position);
        }

        private int CreateFinalCellPosition(ColorType pieceColor, int index)
        {
            string position = $"{(int)pieceColor}{(int)pieceColor}{index}";

            return int.Parse(position);
        }

        private void AddNewPieceMoved(List<PieceDto> piecesMoved, ColorType pieceColor, int nextPosition, int previousPosition)
        {
            piecesMoved.Add(new PieceDto
            {
                Color = pieceColor,
                NextPosition = nextPosition,
                PreviousPosition = previousPosition
            });
        }
    }
}
