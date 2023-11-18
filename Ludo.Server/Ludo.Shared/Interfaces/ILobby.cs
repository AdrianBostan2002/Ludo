using Ludo.Shared.Interfaces;

namespace Ludo.Shared.Entities
{
    public interface ILobby
    {
        int LobbyId { get; set; }
        List<ILobbyParticipant>? Participants { get; set; }
    }
}