using Ludo.Domain.Enums;
using Ludo.Domain.Interfaces;
using Ludo.MediatRPattern.Interfaces;

namespace Ludo.Business.UseCases.Lobby.JoinLobbyUseCase
{
    public class JoinLobbyHandler: IRequestHandler<JoinLobbyRequest, List<ILobbyParticipant>>
    {
        private readonly ILobbyService _lobbyService;

        public JoinLobbyHandler(ILobbyService lobbyService)
        {
            _lobbyService = lobbyService ?? throw new ArgumentNullException(nameof(lobbyService));
        }

        public Task<List<ILobbyParticipant>> Handle(JoinLobbyRequest request)
        {
            ILobbyParticipant newLobbyParticipant = _lobbyService.CreateNewLobbyParticipant(request.Username, RoleType.Regular, request.ConnectionId);

            if (!_lobbyService.JoinLobby(request.LobbyId, newLobbyParticipant))
            {
                throw new Exception("Player couldn't join lobby");
            }

            var lobbyParticipants = _lobbyService.GetLobbyParticipants(request.LobbyId);

            if (lobbyParticipants.Count == 0)
            {
                throw new Exception("Lobby shouldn't be empty");
            }

            return Task.FromResult(lobbyParticipants);
        }
    }
}
