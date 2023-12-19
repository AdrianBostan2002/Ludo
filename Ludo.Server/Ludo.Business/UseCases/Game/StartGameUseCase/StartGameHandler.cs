using Ludo.Domain.Interfaces;
using Ludo.MediatRPattern.Interfaces;

namespace Ludo.Business.UseCases.Game.CreateGameUseCase
{
    public class StartGameHandler: IRequestHandler<StartGameRequest, (IGame, List<IPlayer>)>
    {
        private readonly IGameService _gameService;

        public StartGameHandler(IGameService gameService)
        {
            _gameService = gameService ?? throw new ArgumentNullException(nameof(gameService));
        }

        public Task<(IGame, List<IPlayer>)> Handle(StartGameRequest request)
        {
            if (!_gameService.CheckIfGameCanStart(request.LobbyId))
            {
                throw new Exception("Game can't start");
            }

            IGame game = _gameService.GetGameById(request.LobbyId);

            _gameService.AssignPlayersPiecesRandomColors(game.Players);
            List<IPlayer> playersWithoutCaller = _gameService.GetPlayersWithoutCaller(game, request.ConnectionId);

            (IGame, List<IPlayer>) result = (game, playersWithoutCaller);

            return Task.FromResult(result);
        }
    }
}