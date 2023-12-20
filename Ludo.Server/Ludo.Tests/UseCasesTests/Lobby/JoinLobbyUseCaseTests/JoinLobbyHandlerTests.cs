using Bogus;
using Ludo.Business.UseCases.Lobby.CreateLobby;
using Ludo.Business.UseCases.Lobby.JoinLobbyUseCase;
using Ludo.Domain.Enums;
using Ludo.Domain.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ludo.Tests.UseCasesTests.Lobby.JoinLobbyUseCaseTests
{
    [TestClass]
    public class JoinLobbyHandlerTests
    {
        private readonly Mock<ILobbyService> _mockLobbyService = new Mock<ILobbyService>();

        [TestMethod]
        public void Constructor_HappyFlow_ThrowsNothing()
        {
            Assert.IsNotNull(() => new JoinLobbyHandler(_mockLobbyService.Object));
        }

        public void Constructor_LobbyServiceIsNull_ThrowsArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new JoinLobbyHandler(null));
        }

        public void Handle_HappyFlow_ReturnsRandomLobbyId()
        {
            _mockLobbyService.Setup(x => x.CreateNewLobbyParticipant(It.IsAny<string>(), It.IsAny<RoleType>(), It.IsAny<string>()))
                .Returns(It.IsAny<ILobbyParticipant>());
            _mockLobbyService.Setup(x => x.JoinLobby(It.IsAny<int>(), It.IsAny<ILobbyParticipant>())).Returns(true);
            _mockLobbyService.Setup(x => x.GetLobbyParticipants(It.IsAny<int>())).Returns(new List<ILobbyParticipant>());
            var sut = new JoinLobbyHandler(_mockLobbyService.Object);
            var request = new JoinLobbyRequest { LobbyId = 0, ConnectionId = "", Username = "" };
            var expected = new List<ILobbyParticipant>();

            var response = sut.Handle(request);
            var actual = response.Result;

            Assert.AreEqual(expected, actual);
        }

        public void Handle_HappyFlow_ReturnsRandomLobbyId()
        {
            _mockLobbyService.Setup(x => x.CreateNewLobbyParticipant(It.IsAny<string>(), It.IsAny<RoleType>(), It.IsAny<string>()))
                .Returns(It.IsAny<ILobbyParticipant>());
            _mockLobbyService.Setup(x => x.JoinLobby(It.IsAny<int>(), It.IsAny<ILobbyParticipant>())).Returns(true);
            _mockLobbyService.Setup(x => x.GetLobbyParticipants(It.IsAny<int>())).Returns(new List<ILobbyParticipant>());
            var sut = new JoinLobbyHandler(_mockLobbyService.Object);
            var request = new JoinLobbyRequest { LobbyId = 0, ConnectionId = "", Username = "" };
            var expected = new List<ILobbyParticipant>();

            var response = sut.Handle(request);
            var actual = response.Result;

            Assert.AreEqual(expected, actual);
        }
    }
}
