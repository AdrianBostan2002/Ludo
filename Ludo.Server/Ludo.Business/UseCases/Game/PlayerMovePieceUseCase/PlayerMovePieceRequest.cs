using Ludo.Domain.DTOs;
using Ludo.Domain.Interfaces;
using Ludo.MediatRPattern.Interfaces;

namespace Ludo.Business.UseCases.Game.PlayerMovePieceUseCase
{
    public class PlayerMovePieceRequest: IRequest<(List<PieceDto>, List<IPlayer>, string, List<IPlayer>)>
    {
        public string Username { get; set; }

        public int GameId { get; set; }

        public PieceDto Piece { get; set; }

        public string ConnectionId { get; set; }

        public int DiceNumber { get; set; }
    }
}