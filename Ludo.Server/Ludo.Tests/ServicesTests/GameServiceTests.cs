using Ludo.Business.Options;
using Ludo.Business.Services;
using Ludo.Business.UseCases.Game.PlayerLeaveUseCase;
using Ludo.Domain.Entities;
using Ludo.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ludo.Tests.ServicesTests
{
    [TestClass]
    public class GameServiceTests
    {
        public readonly Mock<IBoardService> _boardService = new Mock<IBoardService>();
        public readonly Mock<IPieceService> _pieceService = new Mock<IPieceService>();
        IOptions<LudoGameOptions> _options;
        public IGameService _gameService;

        public void Initialize()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var services = new ServiceCollection();
            services.Configure<LudoGameOptions>(configuration.GetSection(LudoGameOptions.Key));
            var serviceProvider = services.BuildServiceProvider();

            _options = serviceProvider.GetRequiredService<IOptions<LudoGameOptions>>();
            var ludoOptions = _options.Value;

            _gameService = new GameService(_boardService.Object, _pieceService.Object, _options);
        }

        [TestMethod]
        public void Constructor_ThrowsNothing()
        {
            Initialize();
            Assert.IsNotNull(() => new GameService(_boardService.Object, _pieceService.Object, _options));
        }

        [TestMethod]
        public void AddNewPlayerIntoGame_AddsPlayerToGame()
        {
            Initialize();
            IGame game = new Game();

            IPlayer player = new Player() { Name = "Radu", IsReady = false, ConnectionId = "123" };
            game.Players = new List<IPlayer>() { player };
            var expected = new List<IPlayer>
            {
                new Player { Name = "Radu", IsReady = false, ConnectionId = "123" },
                new Player { Name = "Adrian", IsReady = false, ConnectionId = "123" }
            };

            _gameService.AddNewPlayerIntoGame(game, "Adrian", "123");

            CollectionAssert.AreEqual(game.Players, expected);
        }

        [TestMethod]
        public void GetPlayersWithoutCaller_DoesNotReturnCaller()
        {
            Initialize();
            ILobby lobby = new Lobby() { LobbyId = 123 };
            List<IPlayer> expected = new List<IPlayer>() { new Player { Name = "Adrian", ConnectionId = "101" } };

            _gameService.CreateNewGame(lobby);
            IGame game = _gameService.GetGameById(lobby.LobbyId);
            _gameService.AddNewPlayerIntoGame(game, "Radu", "100");
            _gameService.AddNewPlayerIntoGame(game, "Adrian", "101");

            List<IPlayer> actual = _gameService.GetPlayersWithoutCaller(game, "100");

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CheckIfGameCanStart_ReturnsCorrectBoolean()
        {
            Initialize();
            ILobby lobby = new Lobby() { LobbyId = 123 };
            bool expected = true;

            _gameService.CreateNewGame(lobby);
            IGame game = _gameService.GetGameById(lobby.LobbyId);
            _gameService.AddNewPlayerIntoGame(game, "Radu", "123");
            _gameService.AddNewPlayerIntoGame(game, "Adrian", "123");
            game.Players[0].IsReady = true;
            bool actual = _gameService.CheckIfGameCanStart(game.Id);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetNextDiceRoller_ReturnsCorrectRollerForTwoPlayers()
        {
            Initialize();
            ILobby lobby = new Lobby() { LobbyId = 123 };

            _gameService.CreateNewGame(lobby);
            IGame game = _gameService.GetGameById(lobby.LobbyId);
            _gameService.AddNewPlayerIntoGame(game, "Radu", "100");
            _gameService.AddNewPlayerIntoGame(game, "Adrian", "101");
            _gameService.AssignRandomOrderForRollingDice(game);

            var first = _gameService.GetNextDiceRoller(game);
            var second = _gameService.GetNextDiceRoller(game);

            Assert.AreNotEqual(first, second);
        }
    }
}
