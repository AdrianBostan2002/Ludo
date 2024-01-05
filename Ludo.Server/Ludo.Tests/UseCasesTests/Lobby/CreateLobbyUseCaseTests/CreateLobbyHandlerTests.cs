using Ludo.Business.Options;
using Ludo.Business.Services;
using Ludo.Business.UseCases.Lobby.CreateLobby;
using Ludo.Domain.Enums;
using Ludo.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;

namespace Ludo.Tests.UseCasesTests.Lobby.CreateLobbyUseCaseTests
{
    [TestClass]
    public class CreateLobbyHandlerTests
    {
        private readonly Mock<ILobbyService> _mockLobbyService = new Mock<ILobbyService>();
        IOptions<LudoGameOptions> _options;

        public void InitializeOptions()
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
        }

        [TestMethod]
        public void Constructor_HappyFlow_ThrowsNothing()
        {
            InitializeOptions();
            Assert.IsNotNull(() => new CreateLobbyHandler(_mockLobbyService.Object, _options));
        }

        [TestMethod]
        public void Constructor_LobbyServiceIsNull_ThrowsArgumentNullException()
        {
            InitializeOptions();
            Assert.ThrowsException<ArgumentNullException>(() => new CreateLobbyHandler(null, null));
        }

        [TestMethod]
        public void Handle_HappyFlow_ReturnsRandomLobbyId()
        {
            InitializeOptions();
            var expected = true;
            _mockLobbyService.Setup(x => x.CreateNewLobbyParticipant(It.IsAny<string>(), It.IsAny<RoleType>(), It.IsAny<string>()));
            _mockLobbyService.Setup(x => x.CreateNewLobby(It.IsAny<int>(), It.IsAny<ILobbyParticipant>())).Returns(true);
            var request = CreateRequest();
            var sut = new CreateLobbyHandler(_mockLobbyService.Object, _options);

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
