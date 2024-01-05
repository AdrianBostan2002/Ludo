using Ludo.Business.UseCases.Game.CreateGameUseCase;
using Ludo.Domain.DTOs;
using Ludo.Domain.Entities;
using Ludo.Domain.Interfaces;
using Moq;

namespace Ludo.Tests.UseCasesTests.Game.StartGameUseCaseTests
{
    [TestClass]
    public class StartGameHandlerTests
    {
        private readonly Mock<IGameService> _gameServiceMock = new Mock<IGameService>();
        private readonly Mock<IPieceService> _pieceServiceMock = new Mock<IPieceService>();

        [TestMethod]
        public void Constructor_HappyFlow_ThrowsNothing()
        {
            Assert.IsNotNull(() => new StartGameHandler(_gameServiceMock.Object, _pieceServiceMock.Object));
        }

        [TestMethod]
        public void Constructor_GameServiceIsNull_ThrowsArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new StartGameHandler(null, _pieceServiceMock.Object));
        }

        [TestMethod]
        public void Constructor_PieceServiceIsNull_ThrowsArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new StartGameHandler(_gameServiceMock.Object, null));
        }

        [TestMethod]
        public void Handle_IfGameCannotStart_ThrowsException()
        {
            var request = new StartGameRequest();
            _gameServiceMock.Setup(x => x.CheckIfGameCanStart(It.IsAny<int>())).Returns(false);
            var startGameHandler = new StartGameHandler(_gameServiceMock.Object, _pieceServiceMock.Object);

            Assert.ThrowsException<Exception>(() => startGameHandler.Handle(request));
        }

        [TestMethod]
        public void Handle_IfSearchedGameIsNull_ThrowsException()
        {
            var request = new StartGameRequest();
            IGame nullGame = null;
            _gameServiceMock.Setup(x => x.GetGameById(It.IsAny<int>())).Returns(nullGame);
            var startGameHandler = new StartGameHandler(_gameServiceMock.Object, _pieceServiceMock.Object);

            Assert.ThrowsException<Exception>(() => startGameHandler.Handle(request));
        }

        [TestMethod]
        public void Handle_HappyFlow_ReturnsResult()
        {
            List<Piece> playerPieces, callerPieces;
            IPlayer player;
            IGame game;
            List<PieceDto> pieceDto;
            GameDto expectedGame;
            List<IPlayer> expectedPlayersWithouCaller;

            CreateGameScenario(out playerPieces, out callerPieces, out player, out game, out pieceDto, out expectedGame, out expectedPlayersWithouCaller);

            var request = new StartGameRequest() { ConnectionId = "abc" };
            MockSetup(playerPieces, callerPieces, player, game, pieceDto);
            var startGameHandler = new StartGameHandler(_gameServiceMock.Object, _pieceServiceMock.Object);

            (GameDto actualGameDto, List<IPlayer> actualPlayersWithoutCaller) = startGameHandler.Handle(request).Result;

            Assert.AreEqual(expectedGame, actualGameDto);
            CollectionAssert.AreEqual(expectedPlayersWithouCaller, actualPlayersWithoutCaller);
        }

        private void MockSetup(List<Piece> playerPieces, List<Piece> callerPieces, IPlayer player, IGame game, List<PieceDto> pieceDto)
        {
            _gameServiceMock.Setup(x => x.CheckIfGameCanStart(It.IsAny<int>())).Returns(true);
            _gameServiceMock.Setup(x => x.GetGameById(It.IsAny<int>())).Returns(game);
            _gameServiceMock.Setup(x => x.GetPlayersWithoutCaller(game, "abc")).Returns(new List<IPlayer> { player });
            _gameServiceMock.Setup(x => x.GetNextDiceRoller(game)).Returns(player.ConnectionId);
            _pieceServiceMock.Setup(x => x.AssignPiecesStartPosition(playerPieces)).Returns(pieceDto);
            _pieceServiceMock.Setup(x => x.AssignPiecesStartPosition(callerPieces)).Returns(pieceDto);
        }

        private void CreateGameScenario(out List<Piece> playerPieces, out List<Piece> callerPieces, out IPlayer player, out IGame game, out List<PieceDto> pieceDto, out GameDto expectedGame, out List<IPlayer> expectedPlayersWithouCaller)
        {
            playerPieces = new List<Piece> { new Piece { Color = Domain.Enums.ColorType.Red }, new Piece { Color = Domain.Enums.ColorType.Red } };
            callerPieces = new List<Piece> { new Piece { Color = Domain.Enums.ColorType.Green }, new Piece { Color = Domain.Enums.ColorType.Green } };
            player = new Player { ConnectionId = "123", Name = "Ionut", Pieces = playerPieces, IsReady = true };
            IPlayer caller = new Player { ConnectionId = "abc", Name = "Marian", Pieces = callerPieces, IsReady = true };
            List<IPlayer> players = new List<IPlayer>
            {
                player,
                caller
            };

            game = new Domain.Entities.Game
            {
                Id = 1,
                Players = players,
                Board = new Board() { SpawnPositions = new List<SpawnPieces>() }
            };
            pieceDto = new List<PieceDto>();
            List<PlayerDto> playersDto = new List<PlayerDto>()
            {
                new PlayerDto { Name = "Ionut", IsReady = true, Pieces = pieceDto },
                new PlayerDto { Name = "Marian", IsReady = true, Pieces = pieceDto },
            };

            expectedGame = new GameDto
            {
                Id = 1,
                Players = playersDto,
                FirstDiceRoller = player.Name
            };
            expectedPlayersWithouCaller = new List<IPlayer>() { player };
        }
    }
}