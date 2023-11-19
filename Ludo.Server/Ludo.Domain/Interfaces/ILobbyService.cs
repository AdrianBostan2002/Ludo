namespace Ludo.Domain.Interfaces
{
    public interface ILobbyService
    {
        bool CreateNewLobby(int lobbyId, ILobbyParticipant lobbyOwner);
        public bool JoinLobby(int lobbyId, ILobbyParticipant lobbyParticipant);
        public List<ILobbyParticipant> GetLobbyParticipants(int lobbyId);
        public int LobbiesCount();
        ILobby GetLobbyById(int id);
    }
}