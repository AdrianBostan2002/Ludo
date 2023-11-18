using Ludo.Shared.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace Ludo.DesignPatterns.ObserverPattern.Lobby
{
    public class LobbySubject
    {
        //private List<ILobbyParticipant> _lobbyParticipants;
        //private IHubContext<LobbyHub> _hubContext;

        //public LobbySubject(IHubContext<LobbyHub> hubContext)
        //{
        //    _hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));

        //    _lobbyParticipants = new List<ILobbyParticipant>();
        //}

        //public async Task AddParticipantAsync(ILobbyParticipant lobbyParticipant)
        //{
        //    _lobbyParticipants.Add(lobbyParticipant);

        //    await NotifyAllParticipantsAsync(lobbyParticipant);
        //}

        //public void RemoveParticipant(ILobbyParticipant lobbyParticipant)
        //{
        //    _lobbyParticipants.Remove(lobbyParticipant);
        //}

        //private async Task NotifyAllParticipantsAsync(ILobbyParticipant newLobbyParticipant)
        //{
        //    foreach (var participant in _lobbyParticipants)
        //    {
        //        await _hubContext.Clients.Client(participant.ConnectionId).SendAsync("NewUserJoinedLobby", participant.ConnectionId, newLobbyParticipant);
        //    }
        //}
    }
}