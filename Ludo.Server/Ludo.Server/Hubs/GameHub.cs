using Ludo.Domain.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace Ludo.Server.Hubs
{
    public class GameHub : Hub
    {
        private ILobbyService _lobbyService;
        private IGameService _gameService;

        public GameHub(ILobbyService lobbyService, IGameService gameService)
        {
            _lobbyService = lobbyService ?? throw new ArgumentNullException(nameof(lobbyService));
            _gameService = gameService ?? throw new ArgumentNullException(nameof(gameService));
        }

        //This could transform in CreateNewGameUseCase
        private IGame CreateNewGame(int lobbyId)
        {
            ILobby lobby = _lobbyService.GetLobbyById(lobbyId);

            _gameService.CreateNewGame(lobby);

            IGame game = _gameService.GetGameById(lobbyId);

            return game;
        }

        public Task StartGamePreprocessing(int lobbyId, string username)
        {
            //This could transform into StartGamePreprocessing
            IGame game = _gameService.GetGameById(lobbyId);

            if (game == null)
            {
                game = CreateNewGame(lobbyId);
            }

            _gameService.AddNewPlayerIntoGame(game, username, Context.ConnectionId);

            List<IPlayer> readyPlayers = _gameService.GetReadyPlayers(lobbyId);

            var readyPlayersName = readyPlayers.Select(p => p.Name).ToList();   
            //return readyPlayersName

            return Clients.Caller.SendAsync("PreprocessingSuccessfully", readyPlayersName);
        }

        public Task StartGame(int lobbyId)
        {
            //transform into StartGameUseCase
            if (!_gameService.CheckIfGameCanStart(lobbyId))
            {
                return Clients.Caller.SendAsync("StartGameFailed", new { lobbyId });
            }

            IGame game = _gameService.GetGameById(lobbyId);

            //this will be removed from here
            if (game == null)
            {
                //call CreateNewGameUseCase
                game = CreateNewGame(lobbyId);
            }

            //StartGameUseCase will return IGame
            List<IPlayer> playersWithoutCaller = GetPlayersWithoutCaller(game);

            NotifyPlayersThatNewGameStarted(game, playersWithoutCaller);

            return Clients.Caller.SendAsync("StartGameSucceded", game);
        }

        public Task ReadyToStartGame(int lobbyId, string username)
        {
            //Transform into PlayerIsReadyToStartGameUseCase
            IGame game = _gameService.GetGameById(lobbyId);

            if(game == null)
            {
                game = CreateNewGame(lobbyId);
            }

            IPlayer player = _gameService.GetPlayer(lobbyId, username);

            if (player == null)
            {
                throw new Exception("Player should be in lobby");
            }

            player.IsReady = true;

            //return Game

            List<IPlayer> playersWithoutCaller = GetPlayersWithoutCaller(game);

            NotifyThatNewPlayerIsReady(playersWithoutCaller, username);

            return Clients.Caller.SendAsync("ReadySuccessfully");
        }

        public Task PlayerLeave(int lobbyId, string username)
        {
            bool isRemoved = _gameService.RemovePlayerFromGame(lobbyId, username);

            if(!isRemoved)
            {
                return Clients.Caller.SendAsync("LeavingFailed");
            }

            IGame game = _gameService.GetGameById(lobbyId);

            List<IPlayer> playersWithoutCaller = GetPlayersWithoutCaller(game);

            NotifyPlayersThatSomeoneLeftGame(playersWithoutCaller, username);

            return Clients.Caller.SendAsync("LeavingSucceeded");
        }

        private void NotifyPlayersThatNewGameStarted(IGame game, List<IPlayer> players)
        {
            foreach (var player in players)
            {
                Clients.Client(player.ConnectionId).SendAsync("GameStarted", game);
            }
        }

        private void NotifyThatNewPlayerIsReady(List<IPlayer> players, string username)
        {
            foreach (var player in players)
            {
                Clients.Client(player.ConnectionId).SendAsync("NewPlayerReady", username);
            }
        }

        private List<IPlayer> GetPlayersWithoutCaller(IGame game)
        {
            IEnumerable<IPlayer> caller = game.Players.Where(p => p.ConnectionId.Equals(Context.ConnectionId));

            return game.Players.Except(caller).ToList();
        }

        private void NotifyPlayersThatSomeoneLeftGame(List<IPlayer> players, string username)
        {
            foreach (var player in players)
            {
                Clients.Client(player.ConnectionId).SendAsync("PlayerLeftGame", username);
            }
        }
    }
}