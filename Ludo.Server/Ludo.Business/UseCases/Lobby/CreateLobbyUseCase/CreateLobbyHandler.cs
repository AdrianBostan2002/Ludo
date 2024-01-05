using Ludo.Business.Options;
using Ludo.Domain.Enums;
using Ludo.Domain.Interfaces;
using Ludo.MediatRPattern.Interfaces;
using Microsoft.Extensions.Options;

namespace Ludo.Business.UseCases.Lobby.CreateLobby
{
    public class CreateLobbyHandler : IRequestHandler<CreateLobbyRequest, int>
    {
        private readonly ILobbyService _lobbyService;
        private readonly LudoGameOptions _options;

        public CreateLobbyHandler(ILobbyService lobbyService, IOptions<LudoGameOptions> options)
        {
            _lobbyService = lobbyService ?? throw new ArgumentNullException(nameof(lobbyService));
            _options = options.Value ?? throw new ArgumentNullException(nameof(_options));
        }

        public Task<int> Handle(CreateLobbyRequest request)
        {
            bool newLobbyCreated = false;

            int randomLobbyId = 0;

            while (!newLobbyCreated)
            {
                randomLobbyId = Random.Shared.Next(_options.LobbyIdLowerRange, _options.LobbyIdUpperRange);

                ILobbyParticipant lobbyOwner = _lobbyService.CreateNewLobbyParticipant(request.Username, RoleType.Owner, request.ConnectionId);

                newLobbyCreated = _lobbyService.CreateNewLobby(randomLobbyId, lobbyOwner);
            }

            return Task.FromResult(randomLobbyId);
        }
    }
}