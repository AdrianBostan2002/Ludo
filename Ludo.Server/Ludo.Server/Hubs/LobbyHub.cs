using Ludo.Business.UseCases.Lobby.CreateLobby;
using Ludo.Business.UseCases.Lobby.JoinLobbyUseCase;
using Ludo.Business.UseCases.Lobby.ParticipantLeave;
using Ludo.Domain.Interfaces;
using Ludo.MediatRPattern.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace Ludo.Server.Hubs
{
    public class LobbyHub : Hub
    {
        private readonly IMediator _mediator;

        public LobbyHub(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public Task CreatedLobby(string username)
        {
            var request = new CreateLobbyRequest { Username = username, ConnectionId = Context.ConnectionId };

            Task<int> response = _mediator.Send(request);
            int lobbyId = response.Result;

            return Clients.Caller.SendAsync("JoinedLobby", new { username, lobbyId });
        }

        public Task JoinLobby(int lobbyId, string username)
        {
            try
            {
                var request = new JoinLobbyRequest { Username = username, ConnectionId = Context.ConnectionId, LobbyId = lobbyId };
                var response = _mediator.Send(request);
                var lobbyParticipants = response.Result;

                return NotifyAllParticipantsThatANewUserJoinedLobby(lobbyId, lobbyParticipants);
            }
            catch (Exception)
            {
                return NotifyCallerThatJoiningFailed();
            }
        }

        public Task ParticipantLeave(int lobbyId, string username)
        {
            try
            {
                var request = new ParticipantLeaveRequest { Username = username, LobbyId = lobbyId };
                var response = _mediator.Send(request);
                var lobbyParticipants = response.Result;

                NotifyParticipantsThatSomeoneLeft(lobbyParticipants, username);
                return Clients.Caller.SendAsync("LeaveLobbySucceeded");

            }
            catch (Exception)
            {
                return Clients.Caller.SendAsync("LeaveLobbyFailed");
            }
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
    }
}