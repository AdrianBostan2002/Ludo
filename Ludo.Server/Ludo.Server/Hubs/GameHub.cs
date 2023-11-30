using Ludo.Domain.Entities;
using Ludo.Domain.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace Ludo.Server.Hubs
{
    public class GameHub: Hub
    {
        private ILobbyService _lobbyService;
        private IGameService _gameService;

        public GameHub(ILobbyService lobbyService, IGameService gameService)
        {
            _lobbyService = lobbyService ?? throw new ArgumentNullException(nameof(lobbyService));
            _gameService = gameService ?? throw new ArgumentNullException(nameof(gameService));
        }

        public Task StartGame(int lobbyId)
        {
            if (!_lobbyService.CheckIfGameCanStart(lobbyId))
            {
                return Clients.Caller.SendAsync("StartGameFailed", new { lobbyId });
            }

            ILobby lobby = _lobbyService.GetLobbyById(lobbyId);

            _gameService.CreateNewGame(lobby);

            //Board board = _gameService.GetGameBoard(lobbyId);

            IGame game = _gameService.GetGameById(lobbyId);

            return Clients.Caller.SendAsync("StartGameSucceded", new { game = game });
        }
    }
}
