using Ludo.Shared.Interfaces;

namespace Ludo.DesignPatterns.ObserverPattern.Lobby
{
    public interface ILobbySubject
    {
        Task AddParticipantAsync(ILobbyParticipant lobbyParticipant);
        void RemoveParticipant(ILobbyParticipant lobbyParticipant);
    }
}