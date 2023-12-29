using FluentValidation;
using Ludo.Domain.DTOs;
using Ludo.Domain.Entities;
using Ludo.Domain.Enums;
using Ludo.Domain.Interfaces;
using Ludo.MediatRPattern.Interfaces;

namespace Ludo.Business.UseCases.Game.PlayerMovePieceUseCase
{
    public class PlayerMovePieceHandler : IRequestHandler<PlayerMovePieceRequest, (List<PieceDto>, List<IPlayer>, string)>
    {
        private readonly IGameService _gameService;
        private readonly IPieceService _pieceService;

        public PlayerMovePieceHandler(IGameService gameService, IPieceService pieceService)
        {
            _gameService = gameService ?? throw new ArgumentNullException(nameof(gameService));
            _pieceService = pieceService ?? throw new ArgumentNullException(nameof(pieceService));
        }

        //TODO: Fix problem when trying to take another piece if you both are on the same cell

        public Task<(List<PieceDto>, List<IPlayer>, string)> Handle(PlayerMovePieceRequest request)
        {
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
                MovePieceFromFinalCell(request, game.Board.Cells, piecesMoved, basePosition);
            } //////////////////////////////////////////////////////////
            else
            {
                ICell currentCell = game.Board.Cells[(int)request.Piece.PreviousPosition];
                Piece currentPiece;

                int nextPosition;

                //if(request.Position>=0 && request.Position <= 52)
                //{

                //}
                //is replaced temporary by next if

                currentPiece = currentCell.Pieces.FirstOrDefault();

                if (currentCell.Pieces.Count() != 1)
                {
                    currentPiece = currentCell.Pieces.Where(p => p.Color == request.Piece.Color).FirstOrDefault();
                }

                //Case when Piece is on homecell and I first rolled 6 and I can move piece
                if (CheckIfICanPutMyFirstPieceOnHomeCell(request, basePosition))
                {
                    //should put on the table
                    //but I will pass 6 cells

                    PutPieceOnHomeCell(request, game.Board.Cells, piecesMoved, currentCell);
                }////////////////////////////////////////////////////////////////////
                else/* if (!(basePosition == request.Piece.PreviousPosition))*/
                {
                    //Case when I have at least one piece which is not on home cell

                    //currentPiece = currentCell.Pieces.FirstOrDefault();

                    //if (currentCell.Pieces.Count() != 1)
                    //{
                    //    currentPiece = currentCell.Pieces.Where(p => p.Color == request.Piece.Color).FirstOrDefault();
                    //}

                    nextPosition = ((int)request.Piece.PreviousPosition + request.DiceNumber) % 52;

                    int specialCellUpperBound = (basePosition - 2);

                    if (basePosition == 0)
                    {
                        specialCellUpperBound = 52 - 2;

                        if (nextPosition == 0)
                        {
                            nextPosition = 52;
                        }
                    }

                    int specialCellLowerBound = specialCellUpperBound - 6;

                    //Case when I try to go to a final cell
                    if (CheckIfICanMovePieceOnFinalCell(request, nextPosition, specialCellUpperBound, specialCellLowerBound))
                    {
                        MovePieceOnFinalCell(request, game.Board.Cells, piecesMoved, nextPosition, currentPiece, specialCellUpperBound);
                    }///////////////////////////////////
                    else
                    {
                        //Case when Piece moves normally and I try to replace a new piece
                        
                        RemoveEnemyPiece(request, game, piecesMoved, nextPosition);
                        MovePiece(piecesMoved, game.Board.Cells, currentCell, currentPiece, nextPosition, (int)request.Piece.PreviousPosition);
                    }
                }
            }

            var playersWithouCaller = _gameService.GetPlayersWithoutCaller(game, request.ConnectionId);

            string nextDiceRoller = game.RollDiceOrder.Dequeue();
            game.RollDiceOrder.Enqueue(nextDiceRoller);

            (List<PieceDto> pieces, List<IPlayer> players, string connectionId) result = (piecesMoved, playersWithouCaller, nextDiceRoller);

            return Task.FromResult(result);
        }

        private void RemoveEnemyPiece(PlayerMovePieceRequest request, IGame game, List<PieceDto> piecesMoved, int nextPosition)
        {
            if (!(game.Board.Cells[nextPosition] is HomeCell))
            {
                var differentColorPieces = game.Board.Cells[nextPosition].Pieces.Where(p => p.Color != request.Piece.Color).ToList();

                foreach (var piece in differentColorPieces)
                {
                    int homePosition = _pieceService.GetBasePosition(piece.Color);

                    piecesMoved.Add(new PieceDto
                    {
                        Color = piece.Color,
                        NextPosition = homePosition,
                        PreviousPosition = nextPosition
                    });
                }

                game.Board.Cells[nextPosition].Pieces = game.Board.Cells[nextPosition].Pieces.Except(differentColorPieces).ToList();
            }
        }

        private void MovePiece(List<PieceDto> piecesMoved, List<ICell> boardCells, ICell currentCell, Piece currentPiece, int nextPosition, int previousPosition)
        {
            boardCells[nextPosition].Pieces.Add(currentPiece);

            currentCell.Pieces.Remove(currentPiece);

            AddNewPieceMoved(piecesMoved, currentPiece.Color, nextPosition, previousPosition);
        }

        private void MovePieceOnFinalCell(PlayerMovePieceRequest request, List<ICell> boardCells, List<PieceDto> piecesMoved, int nextPosition, Piece currentPiece, int specialCellUpperBound)
        {
            var specialCell = boardCells[specialCellUpperBound] as SpecialCell;

            specialCell.FinalCells[nextPosition - specialCellUpperBound - 1].Pieces.Add(currentPiece);

            var specialCellNextPosition = CreateFinalCellPosition(currentPiece.Color, nextPosition - specialCellUpperBound);

            AddNewPieceMoved(piecesMoved, currentPiece.Color, specialCellNextPosition, (int)request.Piece.PreviousPosition);
        }

        private static bool CheckIfICanMovePieceOnFinalCell(PlayerMovePieceRequest request, int nextPosition, int specialCellUpperBound, int specialCellLowerBound)
        {
            return request.Piece.PreviousPosition >= specialCellLowerBound &&
                                    request.Piece.PreviousPosition <= specialCellUpperBound &&
                                    nextPosition > specialCellUpperBound;
        }

        private void PutPieceOnHomeCell(PlayerMovePieceRequest request, List<ICell> boardCells, List<PieceDto> piecesMoved, ICell currentCell)
        {
            Piece currentPiece = currentCell.Pieces.FirstOrDefault();
            int nextPosition = (int)request.Piece.PreviousPosition + 1;

            MovePiece(piecesMoved, boardCells, currentCell, currentPiece, nextPosition, (int)request.Piece.PreviousPosition);
        }

        private static bool CheckIfICanPutMyFirstPieceOnHomeCell(PlayerMovePieceRequest request, int basePosition)
        {
            return basePosition == request.Piece.PreviousPosition && request.DiceNumber == 6;
        }

        private void MovePieceFromFinalCell(PlayerMovePieceRequest request, List<ICell> boardCells, List<PieceDto> piecesMoved, int basePosition)
        {
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
                int winningCellNextPosition = CreateWinningCellPosition(currentPiece.Color);
                AddNewPieceMoved(piecesMoved, currentPiece.Color, winningCellNextPosition, (int)request.Piece.PreviousPosition);
            }
            else if (nextPosition % 10 < 5)
            {
                int finalCellNextPosition = CreateFinalCellPosition(currentPiece.Color, nextPosition);
                AddNewPieceMoved(piecesMoved, currentPiece.Color, finalCellNextPosition, (int)request.Piece.PreviousPosition);
            }
        }

        private static bool CheckIfPieceIsOnFinalCell(int previousPosition)
        {
            return previousPosition >= 110 && previousPosition <= 444;
        }

        private int CreateWinningCellPosition(ColorType pieceColor)
        {
            string position = $"{(int)pieceColor}{(int)pieceColor}{(int)pieceColor}{(int)pieceColor}";

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
