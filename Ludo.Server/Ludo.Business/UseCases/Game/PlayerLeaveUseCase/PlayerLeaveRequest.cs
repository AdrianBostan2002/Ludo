using Ludo.Domain.Interfaces;
using Ludo.MediatRPattern.Interfaces;

namespace Ludo.Business.UseCases.Game.PlayerLeaveUseCase
{
    public class PlayerLeaveRequest : IRequest<List<IPlayer>>
    {
        public int LobbyId { get; set; }
        public string Username { get; set; }
        public string ConnectionId { get; set; }
    }
}