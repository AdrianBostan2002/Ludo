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

            int basePosition = _pieceService.GetBasePosition(currentPiece.Color);

            if (basePosition==request.Piece.PreviousPosition && request.DiceNumber == 6)
            {
                //should put on the table
                //but I will pass 6 cells

                currentPiece = currentCell.Pieces.FirstOrDefault();
                nextPosition = (int)request.Piece.PreviousPosition+1;

                game.Board.Cells[nextPosition].Pieces.Add(currentPiece);
                currentCell.Pieces.Remove(currentPiece);

                piecesMoved.Add(new PieceDto
                {
                    Color = currentPiece.Color,
                    NextPosition = nextPosition,
                    PreviousPosition = request.Piece.PreviousPosition
                });
            }
            else if(!(basePosition == request.Piece.PreviousPosition))
            {
                currentPiece = currentCell.Pieces.FirstOrDefault();

                if (currentCell.Pieces.Count() != 1)
                {
                    currentPiece = currentCell.Pieces.Where(p => p.Color == request.Piece.Color).FirstOrDefault();
                }

                nextPosition = ((int)request.Piece.PreviousPosition + request.DiceNumber) % 52;

                game.Board.Cells[nextPosition].Pieces.Add(currentPiece);
                currentCell.Pieces.Remove(currentPiece);

                piecesMoved.Add(new PieceDto
                {
                    Color = currentPiece.Color,
                    NextPosition = nextPosition,
                    PreviousPosition = request.Piece.PreviousPosition
                });
            }

            var playersWithouCaller = _gameService.GetPlayersWithoutCaller(game, request.ConnectionId);

            string nextDiceRoller = game.RollDiceOrder.Dequeue();
            game.RollDiceOrder.Enqueue(nextDiceRoller);

            (List<PieceDto> pieces, List<IPlayer> players, string connectionId) result = (piecesMoved, playersWithouCaller, nextDiceRoller);

            return Task.FromResult(result);
        }
    }
}
