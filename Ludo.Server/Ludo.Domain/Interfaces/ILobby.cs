namespace Ludo.Domain.Interfaces
{
    public interface ILobby
    {
        int LobbyId { get; set; }
        List<ILobbyParticipant>? Participants { get; set; }
    }
}