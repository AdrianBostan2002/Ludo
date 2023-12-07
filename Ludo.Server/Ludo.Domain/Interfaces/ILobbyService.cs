namespace Ludo.Domain.Interfaces
{
    public interface ILobbyService
    {
        bool CreateNewLobby(int lobbyId, ILobbyParticipant lobbyOwner);
        bool JoinLobby(int lobbyId, ILobbyParticipant lobbyParticipant);
        bool RemoveLobbyParticipant(int lobbyId, string username);
        List<ILobbyParticipant> GetLobbyParticipants(int lobbyId);
        int LobbiesCount();
        ILobby GetLobbyById(int id);
    }
}