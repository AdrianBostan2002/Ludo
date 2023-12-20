using Ludo.MediatRPattern.Interfaces;
using Ludo.Server.Hubs;
using Moq;

namespace Ludo.Tests.UseCasesTests.Lobby.HubTests
{
    [TestClass]
    public class CreateLobbyTests
    {
        private Mock<IMediator> _mockMediator = new Mock<IMediator>();

        [TestMethod]
        public void Constructor_HappyFlow_ThrowsNothing()
        {
            Assert.IsNotNull(new LobbyHub(_mockMediator.Object));
        }

        [TestMethod]
        public void Constructor_WhenMediatorIsEmpty_ThrowArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new LobbyHub(null));
        }
    }
}