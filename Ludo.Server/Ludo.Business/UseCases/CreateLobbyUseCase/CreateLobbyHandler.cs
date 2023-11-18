using Ludo.DesignPatterns.ObserverPattern.Lobby;
using Ludo.Domain.Entities;
using Ludo.Domain.Enums;
using Ludo.MediatRPattern.Interfaces;
using Ludo.Shared.Interfaces;

namespace Ludo.Business.UseCases.CreateLobbyUseCase
{
    public class CreateLobbyHandler : IRequestHandler<CreateLobbyRequest, string>
    {
        private readonly ILobbySubject lobbySubject;

        public Task<string> Handle(CreateLobbyRequest request)
        {
            ILobbyParticipant lobbyParticipant = new LobbyParticipant()
            {
                Name = request.Username,
                Role = RoleType.Owner
    //TODO: CONTINUE FROM HERE WITH CONNECTION ID, RETHINK ARCHITECTURE
            };

            return Task.FromResult("Mesaj");
        }
    }
}
