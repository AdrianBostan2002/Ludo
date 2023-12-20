using Ludo.Domain.DTOs;
using Ludo.Domain.Interfaces;
using Ludo.MediatRPattern.Interfaces;

namespace Ludo.Business.UseCases.Game.CreateGameUseCase
{
    public class StartGameHandler: IRequestHandler<StartGameRequest, (GameDto, List<IPlayer>)>
    {
        private readonly IGameService _gameService;

        public StartGameHandler(IGameService gameService)
        {
            _gameService = gameService ?? throw new ArgumentNullException(nameof(gameService));
        }

        public Task<(GameDto, List<IPlayer>)> Handle(StartGameRequest request)
        {
            if (!_gameService.CheckIfGameCanStart(request.LobbyId))
            {
                throw new Exception("Game can't start");
            }

            IGame game = _gameService.GetGameById(request.LobbyId);

            _gameService.AssignPlayersPiecesRandomColors(game.Players);
            List<IPlayer> playersWithoutCaller = _gameService.GetPlayersWithoutCaller(game, request.ConnectionId);

            GameDto gameDto = CreateGameDto(game);

            (GameDto, List<IPlayer>) result = (gameDto, playersWithoutCaller);

            return Task.FromResult(result);
        }

        private GameDto CreateGameDto(IGame game)
        {
            return new GameDto
            {
                Id = game.Id,
                Players = game.Players
            };
        }
    }
}