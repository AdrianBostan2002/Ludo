using Ludo.Domain.Interfaces;
using Ludo.MediatRPattern.Interfaces;

namespace Ludo.Business.UseCases.Lobby.ParticipantLeave
{
    public class ParticipantLeaveRequest: IRequest<List<ILobbyParticipant>>
    {
        public int LobbyId { get; set; }
        public string Username { get; set; }
    }
}