using Ludo.MediatRPattern.Interfaces;

namespace Ludo.Business.UseCases.Lobby.CreateLobby
{
    public class CreateLobbyRequest : IRequest<int>
    {
        public string ConnectionId { get; set; }
        public string Username { get; set; }
    }
}