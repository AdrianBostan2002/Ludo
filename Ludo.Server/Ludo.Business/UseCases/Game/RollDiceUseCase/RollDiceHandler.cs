using Ludo.Domain.Entities;
using Ludo.Domain.Interfaces;
using Ludo.MediatRPattern.Interfaces;

namespace Ludo.Business.UseCases.Game.RollDiceUseCase
{
    public class RollDiceHandler: IRequestHandler<RollDiceRequest, (List<IPlayer>, int)>
    {
        private readonly IGameService _gameService;

        public RollDiceHandler(IGameService gameService)
        {
            _gameService = gameService ?? throw new ArgumentNullException(nameof(gameService));
        }

        public Task<(List<IPlayer>, int)> Handle(RollDiceRequest request)
        {
            int randomNumber = Random.Shared.Next(1, 6);

            IGame game = _gameService.GetGameById(request.GameId);

            if (game == null)
            {
                throw new ArgumentException("Game doesn't exist");
            }

            var playersWithouCaller = _gameService.GetPlayersWithoutCaller(game, request.ConnectionId);

            var result = (playersWithouCaller, randomNumber);

            return Task.FromResult(result);
        }
    }
}
