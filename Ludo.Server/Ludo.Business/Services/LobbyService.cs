using Ludo.Domain.Entities;
using Ludo.Domain.Interfaces;
using System.Collections.Immutable;

namespace Ludo.Business.Services
{
    public class LobbyService : ILobbyService
    {
        private IImmutableDictionary<int, ILobby> _lobbies = ImmutableDictionary<int, ILobby>.Empty;

        public bool CreateNewLobby(int lobbyId, ILobbyParticipant lobbyOwner)
        {
            var newLobby = new Lobby
            {
                Participants = new List<ILobbyParticipant> { lobbyOwner },
                LobbyId = lobbyId
            };

            if (!_lobbies.ContainsKey(lobbyId))
            {
                _lobbies = _lobbies.SetItem(lobbyId, newLobby);

                return true;
            }

            return false;
        }

        public bool JoinLobby(int lobbyId, ILobbyParticipant lobbyParticipant)
        {
            ILobby lobby = GetLobbyById(lobbyId);

            if (lobby == null || lobby.Participants.Count>=4)
            {
                return false;
            }

            lobby.Participants.Add(lobbyParticipant);
            return true;

        }

        public List<ILobbyParticipant> GetLobbyParticipants(int lobbyId)
        {
            ILobby lobby;

            if (_lobbies.TryGetValue(lobbyId, out lobby))
            {
                return lobby.Participants;
            }

            return new List<ILobbyParticipant>();
        }

        public int LobbiesCount()
        {
            return _lobbies.Count();
        }

        public ILobby GetLobbyById(int id)
        {
            ILobby lobby;

            if (_lobbies.TryGetValue(id, out lobby))
            {
                return lobby;
            }

            return null;
        }
    }
}