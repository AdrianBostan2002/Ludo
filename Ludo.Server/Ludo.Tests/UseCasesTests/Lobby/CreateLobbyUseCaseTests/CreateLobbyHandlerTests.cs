using Ludo.Business.UseCases.Lobby.CreateLobby;
using Ludo.Domain.Enums;
using Ludo.Domain.Interfaces;
using Moq;

namespace Ludo.Tests.UseCasesTests.Lobby.CreateLobbyUseCaseTests
{
    [TestClass]
    public class CreateLobbyHandlerTests
    {
        private readonly Mock<ILobbyService> _mockLobbyService = new Mock<ILobbyService>();

        [TestMethod]
        public void Constructor_HappyFlow_ThrowsNothing()
        {
            Assert.IsNotNull(() => new CreateLobbyHandler(_mockLobbyService.Object));
        }

        [TestMethod]
        public void Constructor_LobbyServiceIsNull_ThrowsArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new CreateLobbyHandler(null));
        }

        [TestMethod]
        public void Handle_HappyFlow_ReturnsRandomLobbyId()
        {
            var expected = true;
            _mockLobbyService.Setup(x => x.CreateNewLobbyParticipant(It.IsAny<string>(), It.IsAny<RoleType>(), It.IsAny<string>()));
            _mockLobbyService.Setup(x=>x.CreateNewLobby(It.IsAny<int>(), It.IsAny<ILobbyParticipant>())).Returns(true);
            var request = CreateRequest();
            var sut = new CreateLobbyHandler(_mockLobbyService.Object);

            var response = sut.Handle(request);
            var actual = CheckIfRandomLobbyIdIsValid(response.Result);

            Assert.AreEqual(expected, actual);
        }

        private CreateLobbyRequest CreateRequest()
        {
            return new CreateLobbyRequest { ConnectionId = "", Username = "" };
        }

        private bool CheckIfRandomLobbyIdIsValid(int randomLobbyId)
        {
            return randomLobbyId >= 100 && randomLobbyId <= 999;
        }
    }
}
