using Ludo.Business.Options;
using Ludo.Domain.Entities;
using Ludo.Domain.Enums;
using Ludo.Domain.Interfaces;
using Microsoft.Extensions.Options;
using System.Collections.Immutable;

namespace Ludo.Business.Services
{
    public class LobbyService : ILobbyService
    {
        private IImmutableDictionary<int, ILobby> _lobbies = ImmutableDictionary<int, ILobby>.Empty;
        private readonly LudoGameOptions _options;

        public LobbyService(IOptions<LudoGameOptions> options)
        {
            _options = options.Value ?? throw new ArgumentNullException(nameof(_options));
        }

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

            if (lobby == null || lobby.Participants.Count >= _options.MaxLobbyParticipants)
            {
                return false;
            }

            lobby.Participants.Add(lobbyParticipant);
            return true;
        }

        public bool RemoveLobbyParticipant(int lobbyId, string username)
        {
            ILobby lobby = GetLobbyById(lobbyId);

            if (lobby == null)
            {
                return false;
            }

            ILobbyParticipant lobbyParticipant = lobby.Participants.Where(p => p.Name.Equals(username)).FirstOrDefault();

            if (lobbyParticipant == null)
            {
                return false;
            }

            lobby.Participants.Remove(lobbyParticipant);

            if (lobby.Participants.Count == 0)
            {
                _lobbies = _lobbies.Remove(lobby.LobbyId);
            }

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

        public ILobbyParticipant CreateNewLobbyParticipant(string username, RoleType role, string connectionId)
        {
            ILobbyParticipant lobbyOwner = new LobbyParticipant();
            lobbyOwner.Name = username;
            lobbyOwner.ConnectionId = connectionId;
            lobbyOwner.Role = role;
            return lobbyOwner;
        }
    }
}