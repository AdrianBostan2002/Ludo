using Ludo.Domain.Enums;

namespace Ludo.Domain.Entities
{
    public class LobbyParticipant
    {
        public string Name { get; set; }
        public RoleType Role { get; set; }
    }
}