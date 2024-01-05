using Ludo.Business.Options;
using Ludo.Domain.Entities;
using Ludo.Domain.Enums;
using Ludo.Domain.Interfaces;
using Microsoft.Extensions.Options;
using System.Collections.Immutable;

namespace Ludo.Business.Services
{
    public class GameService : IGameService
    {
        private IImmutableDictionary<int, IGame> _games = ImmutableDictionary<int, IGame>.Empty;
        private readonly LudoGameOptions _options;

        private readonly IBoardService _boardService;
        private readonly IPieceService _pieceService;

        public GameService(IBoardService boardService, IPieceService pieceService, IOptions<LudoGameOptions> options)
        {
            _boardService = boardService ?? throw new ArgumentNullException(nameof(boardService));
            _pieceService = pieceService ?? throw new ArgumentNullException(nameof(pieceService));
            _options = options.Value ?? throw new ArgumentNullException(nameof(_options));
        }

        public void CreateNewGame(ILobby lobby)
        {
            Board board = _boardService.CreateBoard();

            IGame newGame = new Game()
            {
                Id = lobby.LobbyId,
                Board = board,
                Players = new List<IPlayer>(),
                Ranking = new List<IPlayer>()
            };

            _games = _games.Add(lobby.LobbyId, newGame);
        }

        public void AddNewPlayerIntoGame(IGame game, string username, string connectionId)
        {
            Player player = new Player
            {
                Name = username,
                IsReady = false,
                ConnectionId = connectionId,
            };

            game.Players.Add(player);
        }

        public bool RemovePlayerFromGame(int gameId, string username)
        {
            IGame game = GetGameById(gameId);

            if (game == null)
            {
                return false;
            }

            IPlayer player = game.Players.Where(p => p.Name == username).FirstOrDefault();

            if (player == null)
            {
                return false;
            }

            game.Players.Remove(player);

            if (game.Players.Count == 0)
            {
                _games = _games.Remove(game.Id);
            }

            return true;
        }

        public void AssignPlayersPiecesRandomColors(List<IPlayer> players)
        {
            List<ColorType> shuffledColors = ShuflleColorsForPieces();

            foreach (var player in players)
            {
                var pieces = new List<Piece>();

                for (int i = 0; i < _options.PieceMaxNumber; i++)
                {
                    Piece piece = _pieceService.CreatePiece(shuffledColors.Last());

                    pieces.Add(piece);
                }

                shuffledColors.Remove(shuffledColors.Last());

                player.Pieces = pieces;
            }
        }

        private List<ColorType> ShuflleColorsForPieces()
        {
            List<ColorType> colors = new List<ColorType>() { ColorType.Blue, ColorType.Red, ColorType.Green, ColorType.Yellow };

            var random = new Random();

            var shuffledColors = colors.OrderBy(c => random.Next()).ToList();
            return shuffledColors;
        }

        public IGame GetGameById(int id)
        {
            IGame game;

            if (_games.TryGetValue(id, out game))
            {
                return game;
            }

            return null;
        }

        public Board GetGameBoard(int gameId)
        {
            IGame game = GetGameById(gameId);

            return game?.Board;
        }

        public List<IPlayer> GetReadyPlayers(int gameId)
        {
            List<IPlayer> readyPlayers = new List<IPlayer>();

            IGame game = GetGameById(gameId);

            if (game != null)
            {
                readyPlayers = game.Players.Where(p => p.IsReady == true).ToList();
            }

            return readyPlayers;
        }

        public IPlayer GetPlayer(int gameId, string username)
        {
            IGame game = GetGameById(gameId);

            if (game == null)
            {
                return null;
            }

            return game.Players.FirstOrDefault(p => p.Name.Equals(username));
        }

        public bool CheckIfGameCanStart(int gameId)
        {
            IGame game = GetGameById(gameId);

            if (game == null)
            {
                return false;
            }

            if (!(game.Players.Count > 1 && game.Players.Count <= _options.MaxLobbyParticipants))
            {
                return false;
            }

            int playersReadyCount = game.Players.Count(p => p.IsReady == true);

            if (!(playersReadyCount == game.Players.Count - 1))
            {
                return false;
            }

            return true;
        }

        public List<IPlayer> GetPlayersWithoutCaller(IGame game, string connectionId)
        {
            IEnumerable<IPlayer> caller = game.Players.Where(p => p.ConnectionId.Equals(connectionId));

            return game.Players.Except(caller).ToList();
        }

        public void AssignRandomOrderForRollingDice(IGame game)
        {
            var random = new Random();

            var randomOrderPlayersConnectionId = game.Players.OrderBy(c => random.Next()).Select(p => p.ConnectionId);

            LinkedList<string> piecesMoveOrder = new LinkedList<string>(randomOrderPlayersConnectionId);

            game.RollDiceOrder = piecesMoveOrder;
        }

        public bool CheckIfPlayerPiecesAreOnSpawnPosition(IGame game, IPlayer player)
        {
            ColorType playerColor = player.Pieces.FirstOrDefault(p => p != null).Color;

            SpawnPieces? playerPiecesSpawnPosition = game.Board.SpawnPositions.FirstOrDefault(p => p.Color == playerColor);

            return !playerPiecesSpawnPosition.Pieces.Any(p => p == null);
        }

        public bool CheckIfPlayerPicesAreOnTriangleCell(IPlayer player)
        {
            return player.Pieces.Count == 0;
        }

        public void PieceMovedOnWinningCell(IGame game, IPlayer player)
        {
            if (CheckIfPlayerPicesAreOnTriangleCell(player))
            {
                game.Ranking.Add(player);
            }
        }

        public bool CheckIfGameIsFinished(IGame game, IPlayer player)
        {
            if (game.Ranking.Count == game.Players.Count - 1)
            {
                game.Ranking.AddRange(game.Players.Except(game.Ranking));
                return true;
            }

            return false;
        }

        public string GetNextDiceRoller(IGame game)
        {
            string nextDiceRoller = game.RollDiceOrder.First.Value;
            game.RollDiceOrder.RemoveFirst();

            game.RollDiceOrder.AddLast(nextDiceRoller);

            return nextDiceRoller;
        }

        public int GetRandomDiceNumber()
        {
            int randomNumber = Random.Shared.Next(1, 7);

            return randomNumber;
        }
    }
}