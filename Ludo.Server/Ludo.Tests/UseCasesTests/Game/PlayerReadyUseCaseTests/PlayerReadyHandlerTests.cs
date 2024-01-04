using Ludo.Business.UseCases.Game.PlayerReadyUseCase;
using Ludo.Domain.Interfaces;
using Ludo.Domain.Entities;
using Moq;

namespace Ludo.Tests.UseCasesTests.Game.PlayerReadyUseCaseTests
{
    [TestClass]
    public class PlayerReadyHandlerTests
    {
        private readonly Mock<IGameService> _gameServiceMock = new Mock<IGameService>();

        [TestMethod]
        public void Constructor_HappyFlow_ThrowsNothing()
        {
            Assert.IsNotNull(() => new PlayerReadyHandler(_gameServiceMock.Object));
        }

        [TestMethod]
        public void Constructor_GameServiceIsNull_ThrowsArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new PlayerReadyHandler(null));
        }

        [TestMethod]
        public void Handle_IfSearchedGameIsNull_ThrowsException()
        {
            var request = new PlayerReadyRequest();
            IGame nullGame = null;
            _gameServiceMock.Setup(x => x.GetGameById(It.IsAny<int>())).Returns(nullGame);
            var playerReadyHandler = new PlayerReadyHandler(_gameServiceMock.Object);

            Assert.ThrowsException<Exception>(() => playerReadyHandler.Handle(request));
        }

        [TestMethod]
        public void Handle_IfPlayerNotFoundInGame_ThrowException()
        {
            var request = new PlayerReadyRequest();
            IPlayer nullPlayer = null;
            _gameServiceMock.Setup(x => x.GetPlayer(It.IsAny<int>(), It.IsAny<string>())).Returns(nullPlayer);
            var playerReadyHandler = new PlayerReadyHandler(_gameServiceMock.Object);

            Assert.ThrowsException<Exception>(() => playerReadyHandler.Handle(request));
        }

        [TestMethod]
        public async Task Handle_HappyFlow_ReturnsPlayerWithoutCallerAsync()
        {
            IPlayer player = new Player { };
            var expected = new List<IPlayer> { player };

            var request = new PlayerReadyRequest();
            IGame game = new Domain.Entities.Game();
            _gameServiceMock.Setup(x => x.GetGameById(It.IsAny<int>())).Returns(game);
            _gameServiceMock.Setup(x => x.GetPlayer(It.IsAny<int>(), It.IsAny<string>())).Returns(player);
            _gameServiceMock.Setup(x => x.GetPlayersWithoutCaller(game, It.IsAny<string>()))
                .Returns(expected);
            var playerReadyHandler = new PlayerReadyHandler(_gameServiceMock.Object);

            var actual = await playerReadyHandler.Handle(request);

            Assert.AreEqual(expected, actual);
        }
    }
}
