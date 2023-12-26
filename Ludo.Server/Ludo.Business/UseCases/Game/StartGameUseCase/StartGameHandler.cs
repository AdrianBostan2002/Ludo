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
            _gameService.AssignRandomOrderForRollingDice(game);

            PiecesOnStartPositionAddedOnBoard(game);

            List<IPlayer> playersWithoutCaller = _gameService.GetPlayersWithoutCaller(game, request.ConnectionId);

            GameDto gameDto = CreateGameDto(game);

            (GameDto, List<IPlayer>) result = (gameDto, playersWithoutCaller);

            return Task.FromResult(result);
        }

        //TODO: Will be deleted after ui round cells will contain an index 
        public void PiecesOnStartPositionAddedOnBoard(IGame game)
        {
            int GREEN_START_POSITION = 0;
            int YELLOW_START_POSITION = 13;
            int BLUE_START_POSITION = 26;
            int RED_START_POSITION = 39;

            foreach (var player in game.Players)
            {
                var color = player.Pieces.FirstOrDefault().Color;

                int position = 0;
                switch (color)
                {
                    case Domain.Enums.ColorType.Red:
                        position = RED_START_POSITION;
                        break;
                    case Domain.Enums.ColorType.Green:
                        position = GREEN_START_POSITION;
                        break;
                    case Domain.Enums.ColorType.Blue:
                        position = BLUE_START_POSITION;
                        break;
                    case Domain.Enums.ColorType.Yellow:
                        position = YELLOW_START_POSITION;
                        break;
                }

                game.Board.Cells[position].Pieces = player.Pieces;
            }

        }

        private GameDto CreateGameDto(IGame game)
        {
            string firstDiceRollerConnectionId = game.RollDiceOrder.Peek();
            game.RollDiceOrder.Append(firstDiceRollerConnectionId);

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