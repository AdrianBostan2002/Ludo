using Ludo.Business.UseCases.Game.CreateGameUseCase;
using Ludo.Business.UseCases.Game.PlayerLeaveUseCase;
using Ludo.Business.UseCases.Game.StartGamePreprocessing;
using Ludo.Domain.Entities;
using Ludo.Domain.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ludo.Tests.UseCasesTests.Game.StartGamePreprocessingTests
{
    [TestClass]
    public class StartGanePreprocessingTests
    {
        private readonly Mock<IGameService> _gameServiceMock = new Mock<IGameService>();
        private readonly Mock<ILobbyService> _lobbyServiceMock = new Mock<ILobbyService>();

        [TestMethod]
        public void Constructor_HappyFlow_ThrowsNothing()
        {
            Assert.IsNotNull(() => new StartGamePreprocessingHandler(_lobbyServiceMock.Object, _gameServiceMock.Object));
        }

        [TestMethod]
        public void Constructor_GameServiceIsNull_ThrowsArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new StartGamePreprocessingHandler(null, null));
        }

        [TestMethod]
        public async Task Handle_HappyFlow_ReturnsListOfPlayersAsync()
        {
            IPlayer player = new Player() { Name = "Radu", ConnectionId = "123" };
            var players = new List<IPlayer> { player };
            var expectedPlayers = new List<string> { "Radu" };

            var request = new StartGamePreprocessingRequest() { Username = "Radu", ConnectionId = "123" };
            IGame game = null;
            _gameServiceMock.Setup(x => x.GetGameById(It.IsAny<int>())).Returns(game);
            _gameServiceMock.Setup(x => x.GetReadyPlayers(It.IsAny<int>())).Returns(players);
            var startGamePreprocessingHandler = new StartGamePreprocessingHandler(_lobbyServiceMock.Object, _gameServiceMock.Object);

            var actual = await startGamePreprocessingHandler.Handle(request);

            CollectionAssert.AreEqual(expectedPlayers, actual);
        }
    }
}
