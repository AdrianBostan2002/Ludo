using Ludo.Domain.Interfaces;
using Ludo.MediatRPattern.Interfaces;

namespace Ludo.Business.UseCases.Game.StartGamePreprocessing
{
    public class StartGamePreprocessingHandler: IRequestHandler<StartGamePreprocessingRequest, List<string>>
    {
        private readonly ILobbyService _lobbyService;
        private readonly IGameService _gameService;

        public StartGamePreprocessingHandler(ILobbyService lobbyService, IGameService gameService)
        {
            _lobbyService = lobbyService ?? throw new ArgumentNullException(nameof(lobbyService));
            _gameService = gameService ?? throw new ArgumentNullException(nameof(gameService));
        }

        public Task<List<string>> Handle(StartGamePreprocessingRequest request)
        {
            IGame game = _gameService.GetGameById(request.LobbyId);

            if (game == null)
            {
                game = CreateNewGame(request.LobbyId);
            }

            _gameService.AddNewPlayerIntoGame(game, request.Username, request.ConnectionId);
            List<IPlayer> readyPlayers = _gameService.GetReadyPlayers(request.LobbyId);

            List<string> readyPlayersName = readyPlayers.Select(p => p.Name).ToList();

            return Task.FromResult(readyPlayersName);
        }

        private IGame CreateNewGame(int lobbyId)
        {
            ILobby lobby = _lobbyService.GetLobbyById(lobbyId);

            _gameService.CreateNewGame(lobby);

            IGame game = _gameService.GetGameById(lobbyId);

            return game;
        }
    }
}