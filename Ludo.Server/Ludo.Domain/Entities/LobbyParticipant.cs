using Ludo.Domain.Enums;
using Ludo.Shared.Interfaces;

namespace Ludo.Domain.Entities
{
    public class LobbyParticipant: ILobbyParticipant
    {
        public string Name { get; set; }
        public RoleType Role { get; set; }
        public string ConnectionId { get; set; }
    }
}