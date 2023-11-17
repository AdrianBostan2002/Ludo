using System.Collections.Generic;

namespace Ludo.Domain.Entities
{
    public class Lobby
    {
        public int LobbyId { get; set; }
        public List<LobbyParticipant>? Participants { get; set; }
    }
}