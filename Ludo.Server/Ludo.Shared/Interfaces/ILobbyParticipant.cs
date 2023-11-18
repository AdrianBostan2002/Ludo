using Ludo.Domain.Enums;

namespace Ludo.Shared.Interfaces
{
    public interface ILobbyParticipant
    {
        string Name { get; set; }
        RoleType Role { get; set; }
        string ConnectionId { get; set; }   
    }
}