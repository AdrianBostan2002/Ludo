using Ludo.Business.UseCases.Game.PlayerReadyUseCase;
using Ludo.Business.UseCases.Game.RollDiceUseCase;
using Ludo.Domain.Entities;
using Ludo.Domain.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ludo.Tests.UseCasesTests.Game.RollDiceUseCaseTests
{
    [TestClass]
    public class RollDiceHandlerTests
    {
        private readonly Mock<IGameService> _gameServiceMock = new Mock<IGameService>();

        [TestMethod]
        public void Constructor_HappyFlow_ThrowsNothing()
        {
            Assert.IsNotNull(() => new RollDiceHandler(_gameServiceMock.Object));
        }

        [TestMethod]
        public void Constructor_GameServiceIsNull_ThrowsArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new RollDiceHandler(null));
        }

        [TestMethod]
        public void Handle_IfSearchedGameIsNull_ThrowException()
        {
            var request = new RollDiceRequest();
            IGame nullGame = null;
            _gameServiceMock.Setup(x => x.GetGameById(It.IsAny<int>())).Returns(nullGame);
            var rollDiceHandler = new RollDiceHandler(_gameServiceMock.Object);

            Assert.ThrowsException<ArgumentException>(() => rollDiceHandler.Handle(request));
        }

        [TestMethod]
        public async Task Handle_HappyFlow_ReturnsResults()
        {
            IPlayer player = new Player() { ConnectionId = "123" };
            var expectedPlayers = new List<IPlayer> { player };
            int expectedRandomNumber = 1;
            string expectedNextDiceRoller = "";
            bool expectedCanMovePieces = false;
            IGame game = new Domain.Entities.Game();
            game.Players = expectedPlayers;

            var request = new RollDiceRequest() { ConnectionId = "123" };
            
            _gameServiceMock.Setup(x => x.GetGameById(It.IsAny<int>())).Returns(game);
            _gameServiceMock.Setup(x => x.GetRandomDiceNumber()).Returns(expectedRandomNumber);
            _gameServiceMock.Setup(x => x.CheckIfPlayerPiecesAreOnSpawnPosition(It.IsAny<IGame>(), It.IsAny<IPlayer>())).Returns(true);
            _gameServiceMock.Setup(x => x.GetNextDiceRoller(It.IsAny<IGame>())).Returns(expectedNextDiceRoller);
            _gameServiceMock.Setup(x => x.GetPlayersWithoutCaller(game, It.IsAny<string>())).Returns(expectedPlayers);

            var rollDiceHandler = new RollDiceHandler(_gameServiceMock.Object);

            (List<IPlayer> actualPlayers, int actualRandomNumber, string actualNextDiceRoller, bool actualCanMovePieces) = await rollDiceHandler.Handle(request);

            Assert.AreEqual(actualPlayers, expectedPlayers);
            Assert.AreEqual(actualRandomNumber, expectedRandomNumber);
            Assert.AreEqual(actualNextDiceRoller, expectedNextDiceRoller);
            Assert.AreEqual(actualCanMovePieces, expectedCanMovePieces);
        }
    }
}
