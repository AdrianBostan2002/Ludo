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
            //TransformIntoCreateLobbyUseCase
            bool newLobbyCreated = false;

            int randomLobbyId = 0;

            while (!newLobbyCreated)
            {
                randomLobbyId = Random.Shared.Next(100, 999);

                ILobbyParticipant lobbyOwner = CreateNewLobbyParticipant(username, RoleType.Owner);

                newLobbyCreated = _lobbyService.CreateNewLobby(randomLobbyId, lobbyOwner);
            }

            //Return username, randomLobbyId

            return Clients.Caller.SendAsync("JoinedLobby", new { username, lobbyId = randomLobbyId });
        }

        public Task JoinLobby(int lobbyId, string username)
        {
            //Transform into JoinLobbyUseCase
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
            //return lobbyId, lobbyParticipants

            return NotifyAllParticipantsThatANewUserJoinedLobby(lobbyId, lobbyParticipants);
        }

        public Task ParticipantLeave(int lobbyId, string username)
        {
            bool isRemoved = _lobbyService.RemoveLobbyParticipant(lobbyId, username);

            if (!isRemoved)
            {
                return Clients.Caller.SendAsync("LeaveLobbyFailed");
            }

            var lobbyParticipants = _lobbyService.GetLobbyParticipants(lobbyId);

            NotifyParticipantsThatSomeoneLeft(lobbyParticipants, username);
            return Clients.Caller.SendAsync("LeaveLobbySucceeded");
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

            return Clients.Client(lastParticipant.ConnectionId).SendAsync("SuccessfullyContectedToLobby", new { lobbyParticipants, lobbyId });
        }

        private Task NotifyCallerThatJoiningSucceded(ILobby lobby)
        {
            return Clients.Caller.SendAsync("SuccessfullyContectedToLobby", lobby.Participants);
        }

        private Task NotifyCallerThatJoiningFailed()
        {
            return Clients.Caller.SendAsync("UnSuccessfullyContectedToLobby");
        }

        private void NotifyParticipantsThatSomeoneLeft(List<ILobbyParticipant> lobbyParticipants, string username)
        {
            foreach (var participant in lobbyParticipants)
            {
                Clients.Client(participant.ConnectionId).SendAsync("PlayerLeftLobby", username);
            }
        }

        //This method will be moved into lobbyService
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