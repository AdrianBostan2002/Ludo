using Ludo.Domain.Interfaces;
using Ludo.MediatRPattern.Interfaces;

namespace Ludo.Business.UseCases.Game.RollDiceUseCase
{
    public class RollDiceHandler : IRequestHandler<RollDiceRequest, (List<IPlayer>, int, string, bool)>
    {
        private readonly IGameService _gameService;

        public RollDiceHandler(IGameService gameService)
        {
            _gameService = gameService ?? throw new ArgumentNullException(nameof(gameService));
        }

        public Task<(List<IPlayer>, int, string, bool)> Handle(RollDiceRequest request)
        {
            int randomNumber = Random.Shared.Next(1, 7);

            IGame game = _gameService.GetGameById(request.GameId);

            if (game == null)
            {
                throw new ArgumentException("Game doesn't exist");
            }

            var callerPlayer = game.Players.Where(p => p.ConnectionId.Equals(request.ConnectionId)).FirstOrDefault();

            string nextDiceRoller = "";
            bool canMovePieces = true;

            if (randomNumber != 6 && _gameService.CheckIfPlayerPiecesAreOnStartPosition(game, callerPlayer))
            {
                nextDiceRoller = game.RollDiceOrder.Dequeue();
                game.RollDiceOrder.Enqueue(nextDiceRoller);
                canMovePieces = false;
            }

            var playersWithouCaller = _gameService.GetPlayersWithoutCaller(game, request.ConnectionId);

            var result = (playersWithouCaller, randomNumber, nextDiceRoller, canMovePieces);

            return Task.FromResult(result);
        }
    }
}
