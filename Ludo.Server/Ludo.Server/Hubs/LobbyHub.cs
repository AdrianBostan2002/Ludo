using Ludo.Domain.Entities;
using Ludo.Domain.Enums;
using Ludo.Domain.Interfaces;
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

            int randomLobbyId = 0;

            while (!newLobbyCreated)
            {
                randomLobbyId = Random.Shared.Next(100, 999);

                ILobbyParticipant lobbyOwner = CreateNewLobbyParticipant(username, RoleType.Owner);

                newLobbyCreated = _lobbyService.CreateNewLobby(randomLobbyId, lobbyOwner);
            }

            return Clients.Caller.SendAsync("NewUserJoined", new { username, randomLobbyId });
        }

        public Task JoinLobby(int lobbyId, string username)
        {
            ILobbyParticipant newLobbyParticipant = CreateNewLobbyParticipant(username, RoleType.Regular);
            
            if (!_lobbyService.JoinLobby(lobbyId, newLobbyParticipant))
            {
                return NotifyCallerThatJoiningFailed();
            }

            var lobbyParticipants = _lobbyService.GetLobbyParticipants(lobbyId);

            if (lobbyParticipants.Count == 0)
            {
                throw new Exception("Lobby shouldn't be empty");
            }

            return NotifyAllParticipantsThatANewUserJoinedLobby(lobbyId, lobbyParticipants);
        }

        private Task NotifyAllParticipantsThatANewUserJoinedLobby(int lobbyId, List<ILobbyParticipant> lobbyParticipants)
        {
            var lastParticipant = lobbyParticipants.LastOrDefault();

            if (lastParticipant == null)
            {
                return NotifyCallerThatJoiningFailed();
            }

            for (int i = 0; i < lobbyParticipants.Count - 1; i++)
            {
                Clients.Client(lobbyParticipants[i].ConnectionId).SendAsync("NewUserJoined", new { username = lastParticipant.Name, randomLobbyId = lobbyId });
            }

            return Clients.Client(lastParticipant.ConnectionId).SendAsync("SuccessfullyContectedToLobby", lobbyParticipants);
        }

        private Task NotifyCallerThatJoiningSucceded(ILobby lobby)
        {
            return Clients.Caller.SendAsync("SuccessfullyContectedToLobby", lobby.Participants);
        }

        private Task NotifyCallerThatJoiningFailed()
        {
            return Clients.Caller.SendAsync("UnSuccessfullyContectedToLobby");
        }

        private ILobbyParticipant CreateNewLobbyParticipant(string username, RoleType role)
        {
            ILobbyParticipant lobbyOwner = new LobbyParticipant();
            lobbyOwner.Name = username;
            lobbyOwner.ConnectionId = Context.ConnectionId;
            lobbyOwner.Role = role;
            return lobbyOwner;
        }
    }
}