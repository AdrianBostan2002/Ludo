using Ludo.Business.UseCases.Game.PlayerLeaveUseCase;
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

namespace Ludo.Tests.UseCasesTests.Game.PlayerLeaveHandlerTests
{
    [TestClass]
    public class PlayerLeaveHandlerTests
    {
        private readonly Mock<IGameService> _gameServiceMock = new Mock<IGameService>();

        [TestMethod]
        public void Constructor_HappyFlow_ThrowsNothing()
        {
            Assert.IsNotNull(() => new PlayerLeaveHandler(_gameServiceMock.Object));
        }

        [TestMethod]
        public void Constructor_GameServiceIsNull_ThrowsArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new PlayerLeaveHandler(null));
        }

        [TestMethod]
        public void Handle_IfSearchedGameIsNull_ThrowException()
        {
            var request = new PlayerLeaveRequest();
            _gameServiceMock.Setup(x => x.RemovePlayerFromGame(It.IsAny<int>(), It.IsAny<string>())).Returns(false);
            var playerLeaveHandler = new PlayerLeaveHandler(_gameServiceMock.Object);

            Assert.ThrowsException<Exception>(() => playerLeaveHandler.Handle(request));
        }

        [TestMethod]
        public async Task Handle_HappyFlow_ReturnsPlayerWithoutCallerAsync()
        {
            IPlayer player = new Player { };
            var expected = new List<IPlayer> { player };

            var request = new PlayerLeaveRequest();
            IGame game = new Domain.Entities.Game();
            _gameServiceMock.Setup(x => x.RemovePlayerFromGame(It.IsAny<int>(), It.IsAny<string>())).Returns(true);
            _gameServiceMock.Setup(x => x.GetGameById(It.IsAny<int>())).Returns(game);
            _gameServiceMock.Setup(x => x.GetPlayersWithoutCaller(game, It.IsAny<string>())).Returns(expected);
            var playerLeaveHandler = new PlayerLeaveHandler(_gameServiceMock.Object);

            var actual = await playerLeaveHandler.Handle(request);

            Assert.AreEqual(expected, actual);
        }
    }
}
