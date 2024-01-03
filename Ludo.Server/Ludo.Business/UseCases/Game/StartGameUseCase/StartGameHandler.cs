using Ludo.Domain.DTOs;
using Ludo.Domain.Entities;
using Ludo.Domain.Interfaces;
using Ludo.MediatRPattern.Interfaces;

namespace Ludo.Business.UseCases.Game.CreateGameUseCase
{
    public class StartGameHandler : IRequestHandler<StartGameRequest, (GameDto, List<IPlayer>)>
    {
        private readonly IGameService _gameService;
        private readonly IPieceService _pieceService;

        public StartGameHandler(IGameService gameService, IPieceService pieceService)
        {
            _gameService = gameService ?? throw new ArgumentNullException(nameof(gameService));
            _pieceService = pieceService ?? throw new ArgumentNullException(nameof(pieceService));
        }

        public Task<(GameDto, List<IPlayer>)> Handle(StartGameRequest request)
        {
            if (!_gameService.CheckIfGameCanStart(request.LobbyId))
            {
                throw new Exception("Game can't start");
            }

            IGame game = _gameService.GetGameById(request.LobbyId);

            _gameService.AssignPlayersPiecesRandomColors(game.Players);

            game.Board.SpawnPositions = new List<SpawnPieces>();

            foreach (var player in game.Players)
            {
                game.Board.SpawnPositions.Add(new SpawnPieces
                {
                    Color = player.Pieces[0].Color,
                    Pieces = new List<Piece>(player.Pieces)
                });
            }

            _gameService.AssignRandomOrderForRollingDice(game);

            List<IPlayer> playersWithoutCaller = _gameService.GetPlayersWithoutCaller(game, request.ConnectionId);

            GameDto gameDto = CreateGameDto(game);

            (GameDto, List<IPlayer>) result = (gameDto, playersWithoutCaller);

            return Task.FromResult(result);
        }

        private GameDto CreateGameDto(IGame game)
        {
            string firstDiceRollerConnectionId = _gameService.GetNextDiceRoller(game);

            var firstDiceRollerName = game.Players.Where(p => p.ConnectionId.Equals(firstDiceRollerConnectionId))
                .Select(p => p.Name)
                .FirstOrDefault();

            List<PlayerDto> playersDto = new List<PlayerDto>();

            foreach (var player in game.Players)
            {
                List<PieceDto> piecesDto = _pieceService.AssignPiecesStartPosition(player.Pieces);

                PlayerDto playerDto = new PlayerDto
                {
                    IsReady = player.IsReady,
                    Name = player.Name,
                    Pieces = piecesDto
                };

                playersDto.Add(playerDto);
            }

            return new GameDto
            {
                Id = game.Id,
                Players = playersDto,
                FirstDiceRoller = firstDiceRollerName
            };
        }
    }
}