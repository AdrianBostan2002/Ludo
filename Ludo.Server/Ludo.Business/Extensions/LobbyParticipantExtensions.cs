using Ludo.Domain.Entities;
using Ludo.Domain.Interfaces;

namespace Ludo.Business.Extensions
{
    public static class LobbyParticipantExtensions
    {
        public static IPlayer ToPlayer(this ILobbyParticipant lobbyParticipant)
        {
            return new Player()
            {
                Name = lobbyParticipant.Name,
                ConnectionId = lobbyParticipant.ConnectionId
            };
        }
    }
}