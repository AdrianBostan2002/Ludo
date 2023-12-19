using Ludo.Domain.Interfaces;
using Ludo.MediatRPattern.Interfaces;

namespace Ludo.Business.UseCases.Game.PlayerReadyUseCase
{
    public class PlayerReadyHandler : IRequestHandler<PlayerReadyRequest, List<IPlayer>>
    {
        private readonly IGameService _gameService;

        public PlayerReadyHandler(IGameService gameService)
        {
            _gameService = gameService ?? throw new ArgumentNullException(nameof(gameService));
        }

        public Task<List<IPlayer>> Handle(PlayerReadyRequest request)
        {
            IGame game = _gameService.GetGameById(request.LobbyId);

            if (game == null)
            {
                throw new Exception("Game doesn't exist");
            }

            IPlayer player = _gameService.GetPlayer(request.LobbyId, request.Username);

            if (player == null)
            {
                throw new Exception("Player should be in lobby");
            }

            player.IsReady = true;

            List<IPlayer> playersWithoutCaller = _gameService.GetPlayersWithoutCaller(game, request.ConnectionId);

            return Task.FromResult(playersWithoutCaller);
        }
    }
}