using Ludo.Domain.Interfaces;
using Ludo.MediatRPattern.Interfaces;

namespace Ludo.Business.UseCases.Game.PlayerReadyUseCase
{
    public class PlayerReadyRequest : IRequest<List<IPlayer>>
    {
        public int LobbyId { get; set; }
        public string Username { get; set; }
        public string ConnectionId { get; set; }
    }
}
