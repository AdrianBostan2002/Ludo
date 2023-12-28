using FluentValidation;
using Ludo.Domain.DTOs;
using Ludo.Domain.Entities;
using Ludo.Domain.Enums;
using Ludo.Domain.Interfaces;
using Ludo.MediatRPattern.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public Task<(List<PieceDto>, List<IPlayer>, string)> Handle(PlayerMovePieceRequest request)
        {
            IGame game = _gameService.GetGameById(request.GameId);

            if (game == null)
            {
                throw new Exception("Game doesn't exists");
            }

            List<PieceDto> piecesMoved = new List<PieceDto>();
            int nextPosition;
            Piece currentPiece;

            int basePosition = _pieceService.GetBasePosition(request.Piece.Color);

            if (request.Piece.PreviousPosition >= 110 && request.Piece.PreviousPosition <= 444)
            {
                nextPosition = (int)(request.Piece.PreviousPosition + request.DiceNumber);

                if (nextPosition % 10 >= 5)
                {
                    //Piece is on triangle cell
                }
                else
                {
                    int specialCellsPosition = basePosition - 2;

                    SpecialCell specialCells = game.Board.Cells[specialCellsPosition] as SpecialCell;

                    var specialCell = specialCells.FinalCells[(int)(request.Piece.PreviousPosition % 10)];

                    currentPiece = specialCell.Pieces.Where(p => p.Color == request.Piece.Color).FirstOrDefault();

                    piecesMoved.Add(new PieceDto
                    {
                        Color = currentPiece.Color,
                        NextPosition = CreateFinalCellPosition(currentPiece.Color, nextPosition),
                        PreviousPosition = request.Piece.PreviousPosition
                    });
                }
            }
            else
            {
                ICell currentCell = game.Board.Cells[(int)request.Piece.PreviousPosition];


                //if(request.Position>=0 && request.Position <= 52)
                //{

                //}
                //is replaced temporary by next if

                currentPiece = currentCell.Pieces.FirstOrDefault();

                if (currentCell.Pieces.Count() != 1)
                {
                    currentPiece = currentCell.Pieces.Where(p => p.Color == request.Piece.Color).FirstOrDefault();
                }

                if (basePosition == request.Piece.PreviousPosition && request.DiceNumber == 6)
                {
                    //should put on the table
                    //but I will pass 6 cells

                    currentPiece = currentCell.Pieces.FirstOrDefault();
                    nextPosition = (int)request.Piece.PreviousPosition + 1;

                    game.Board.Cells[nextPosition].Pieces.Add(currentPiece);
                    currentCell.Pieces.Remove(currentPiece);

                    piecesMoved.Add(new PieceDto
                    {
                        Color = currentPiece.Color,
                        NextPosition = nextPosition,
                        PreviousPosition = request.Piece.PreviousPosition
                    });
                }
                else if (!(basePosition == request.Piece.PreviousPosition))
                {
                    currentPiece = currentCell.Pieces.FirstOrDefault();

                    if (currentCell.Pieces.Count() != 1)
                    {
                        currentPiece = currentCell.Pieces.Where(p => p.Color == request.Piece.Color).FirstOrDefault();
                    }

                    nextPosition = ((int)request.Piece.PreviousPosition + request.DiceNumber) % 52;

                    int specialCellUpperBound = basePosition + 6;
                    int specialCellLowerBound = (basePosition - 2)%52;

                    if (basePosition == 0)
                    {
                        specialCellLowerBound = 50;
                    }

                    //TODO: Find a condition for the case when I traversed entire board

                    if (request.Piece.PreviousPosition >= specialCellLowerBound &&
                        request.Piece.PreviousPosition <= specialCellUpperBound
                       )
                    {
                        var specialCell = game.Board.Cells[specialCellUpperBound] as SpecialCell;

                        specialCell.FinalCells.FirstOrDefault().Pieces.Add(currentPiece);

                        var specialCellNextPosition = CreateFinalCellPosition(currentPiece.Color, 0);

                        piecesMoved.Add(new PieceDto
                        {
                            Color = currentPiece.Color,
                            NextPosition = nextPosition,
                            PreviousPosition = request.Piece.PreviousPosition
                        });
                    }
                    else
                    {
                        game.Board.Cells[nextPosition].Pieces.Add(currentPiece);
                        currentCell.Pieces.Remove(currentPiece);

                        piecesMoved.Add(new PieceDto
                        {
                            Color = currentPiece.Color,
                            NextPosition = nextPosition,
                            PreviousPosition = request.Piece.PreviousPosition
                        });
                    }
                }
            }

            var playersWithouCaller = _gameService.GetPlayersWithoutCaller(game, request.ConnectionId);

            string nextDiceRoller = game.RollDiceOrder.Dequeue();
            game.RollDiceOrder.Enqueue(nextDiceRoller);

            (List<PieceDto> pieces, List<IPlayer> players, string connectionId) result = (piecesMoved, playersWithouCaller, nextDiceRoller);

            return Task.FromResult(result);
        }

        private int CreateFinalCellPosition(ColorType pieceColor, int index)
        {
            string position = $"{(int)pieceColor}{(int)pieceColor}{index}";

            return int.Parse(position);
        }
    }
}
