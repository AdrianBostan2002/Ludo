using Ludo.Domain.DTOs;
using Ludo.MediatRPattern.Interfaces;

namespace Ludo.Business.UseCases.Game.PlayerMovePieceUseCase
{
    public class PlayerMovePieceRequest: IRequest<List<PieceDto>>
    {
        public string Username { get; set; }

        public int Position { get; set; }
        
        public int DiceNumber { get; set; }
    }
}