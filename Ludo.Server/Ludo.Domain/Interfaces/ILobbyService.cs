namespace Ludo.Domain.Interfaces
{
    public interface ILobbyService
    {
        bool CreateNewLobby(int lobbyId, ILobbyParticipant lobbyOwner);
        bool JoinLobby(int lobbyId, ILobbyParticipant lobbyParticipant);
        List<ILobbyParticipant> GetLobbyParticipants(int lobbyId);
        int LobbiesCount();
        ILobby GetLobbyById(int id);
    }
}