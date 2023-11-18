using Ludo.Domain.Entities;
using Ludo.Domain.Enums;
using Ludo.Shared.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace Ludo.Server.Hubs
{
    public class LobbyHub : Hub
    {
        private ILobbyService _lobbyService;

        public LobbyHub(ILobbyService lobbyService)
        {
            _lobbyService = lobbyService ?? throw new ArgumentNullException(nameof(lobbyService));
        }

        public async Task Test(string[] yourMessage) =>
        await Clients.All.SendAsync("your message", yourMessage);

        public Task CreatedLobby(string username)
        {
            bool newLobbyCreated = false;

            while (!newLobbyCreated)
            {
                var randomLobbyId = Random.Shared.Next(100, 999);

                ILobbyParticipant lobbyOwner = new LobbyParticipant();
                lobbyOwner.Name = username;
                lobbyOwner.ConnectionId = Context.ConnectionId;
                lobbyOwner.Role = RoleType.Owner;

                newLobbyCreated = _lobbyService.CreateNewLobby(randomLobbyId, lobbyOwner);
            }

            return Clients.Caller.SendAsync("NewUserJoined", username);
        }

        public Task JoinLobby(int lobbyId, string username)
        {
            string connectionId = Context.ConnectionId;

            ILobbyParticipant newLobbyParticipant = new LobbyParticipant();
            newLobbyParticipant.Name = username;
            newLobbyParticipant.ConnectionId = connectionId;
            newLobbyParticipant.Role = RoleType.Regular;

            if(!_lobbyService.JoinLobby(lobbyId, newLobbyParticipant))
            {
                return NotifyCallerThatJoiningFailed();
            }

            var lobbyParticipants = _lobbyService.GetLobbyParticipants(lobbyId);

            if(lobbyParticipants.Count == 0)
            {
                throw new Exception("Lobby shouldn't be empty");
            }

            return NotifyAllParticipantsThatANewUserJoinedLobby(lobbyId, lobbyParticipants);
        }

        private Task NotifyAllParticipantsThatANewUserJoinedLobby(int lobbyId, List<ILobbyParticipant> lobbyParticipants)
        {
            var lastParticipant = lobbyParticipants.LastOrDefault();

            if(lastParticipant == null)
            {
                return NotifyCallerThatJoiningFailed();
            }

            for (int i = 0; i < lobbyParticipants.Count-1; i++)
            {
                Clients.Client(lobbyParticipants[i].ConnectionId).SendAsync("NewUserJoined", lastParticipant.Name);
            }

            return Clients.Client(lastParticipant.ConnectionId).SendAsync("SuccessfullyContectedToLobby", lobbyParticipants);
        }

        private Task NotifyCallerThatJoiningSucceded(int lobbyId)
        {
            var allLobbyParticipants = _lobbyService.GetLobbyParticipants(lobbyId);

            return Clients.Caller.SendAsync("SuccessfullyContectedToLobby", allLobbyParticipants);
        }

        private Task NotifyCallerThatJoiningFailed()
        {
            return Clients.Caller.SendAsync("UnSuccessfullyContectedToLobby");
        }
    }
}