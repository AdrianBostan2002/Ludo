using Ludo.Domain.Interfaces;
using Ludo.MediatRPattern.Interfaces;

namespace Ludo.Business.UseCases.Game.PlayerLeaveUseCase
{
    public class PlayerLeaveHandler : IRequestHandler<PlayerLeaveRequest, List<IPlayer>>
    {
        private readonly IGameService _gameService;

        public PlayerLeaveHandler(IGameService gameService)
        {
            _gameService = gameService ?? throw new ArgumentNullException(nameof(gameService));
        }

        public Task<List<IPlayer>> Handle(PlayerLeaveRequest request)
        {
            bool isRemoved = _gameService.RemovePlayerFromGame(request.LobbyId, request.Username);

            if (!isRemoved)
            {
                throw new Exception("Player couldn't be removed");
            }

            IGame game = _gameService.GetGameById(request.LobbyId);

            List<IPlayer> playersWithoutCaller = new List<IPlayer>();

            if (game != null)
            {
                playersWithoutCaller = _gameService.GetPlayersWithoutCaller(game, request.ConnectionId);
            }

            return Task.FromResult(playersWithoutCaller);
        }
    }
}