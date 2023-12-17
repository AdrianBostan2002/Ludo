using Ludo.Domain.Interfaces;
using Ludo.MediatRPattern.Interfaces;

namespace Ludo.Business.UseCases.Lobby.JoinLobbyUseCase
{
    public class JoinLobbyRequest: IRequest<List<ILobbyParticipant>>
    {
        public int LobbyId { get; set; }
        public string Username { get ; set; }
        public string ConnectionId { get; set; }
    }
}