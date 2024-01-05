using Ludo.Domain.Interfaces;
using Ludo.MediatRPattern.Interfaces;

namespace Ludo.Business.UseCases.Lobby.ParticipantLeave
{
    public class ParticipantLeaveHandler : IRequestHandler<ParticipantLeaveRequest, List<ILobbyParticipant>>
    {
        private readonly ILobbyService _lobbyService;

        public ParticipantLeaveHandler(ILobbyService lobbyService)
        {
            _lobbyService = lobbyService ?? throw new ArgumentNullException(nameof(lobbyService));
        }

        public Task<List<ILobbyParticipant>> Handle(ParticipantLeaveRequest request)
        {
            bool isRemoved = _lobbyService.RemoveLobbyParticipant(request.LobbyId, request.Username);

            if (!isRemoved)
            {
                throw new Exception("Player is not in lobby");
            }

            List<ILobbyParticipant> lobbyParticipants = new List<ILobbyParticipant>();

            ILobby lobby = _lobbyService.GetLobbyById(request.LobbyId);

            if (lobby != null)
            {
                lobbyParticipants = _lobbyService.GetLobbyParticipants(request.LobbyId);
            }

            return Task.FromResult(lobbyParticipants);
        }
    }
}