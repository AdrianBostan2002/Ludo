using Ludo.Domain.Interfaces;

namespace Ludo.Domain.Entities
{
    public class Lobby : ILobby
    {
        public int LobbyId { get; set; }
        public List<ILobbyParticipant>? Participants { get; set; }
    }
}