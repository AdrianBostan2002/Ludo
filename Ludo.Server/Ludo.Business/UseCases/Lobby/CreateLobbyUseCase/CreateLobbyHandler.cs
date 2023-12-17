using Ludo.Domain.Enums;
using Ludo.Domain.Interfaces;
using Ludo.MediatRPattern.Interfaces;

namespace Ludo.Business.UseCases.Lobby.CreateLobby
{
    public class CreateLobbyHandler : IRequestHandler<CreateLobbyRequest, int>
    {
        private readonly ILobbyService _lobbyService;

        public CreateLobbyHandler(ILobbyService lobbyService)
        {
            _lobbyService = lobbyService ?? throw new ArgumentNullException(nameof(lobbyService));
        }

        public Task<int> Handle(CreateLobbyRequest request)
        {
            bool newLobbyCreated = false;

            int randomLobbyId = 0;

            while (!newLobbyCreated)
            {
                randomLobbyId = Random.Shared.Next(100, 999);

                ILobbyParticipant lobbyOwner = _lobbyService.CreateNewLobbyParticipant(request.Username, RoleType.Owner, request.ConnectionId);

                newLobbyCreated = _lobbyService.CreateNewLobby(randomLobbyId, lobbyOwner);
            }

            return Task.FromResult(randomLobbyId);
        }
    }
}