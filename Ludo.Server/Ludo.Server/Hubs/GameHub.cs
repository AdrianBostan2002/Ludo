using Ludo.Business.UseCases.Game.CreateGameUseCase;
using Ludo.Business.UseCases.Game.PlayerLeaveUseCase;
using Ludo.Business.UseCases.Game.PlayerMovePieceUseCase;
using Ludo.Business.UseCases.Game.PlayerReadyUseCase;
using Ludo.Business.UseCases.Game.RollDiceUseCase;
using Ludo.Business.UseCases.Game.StartGamePreprocessing;
using Ludo.Domain.DTOs;
using Ludo.Domain.Enums;
using Ludo.Domain.Interfaces;
using Ludo.MediatRPattern.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace Ludo.Server.Hubs
{
    public class GameHub : Hub
    {
        private readonly IMediator _mediator;

        public GameHub(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public Task StartGamePreprocessing(int lobbyId, string username)
        {
            var request = new StartGamePreprocessingRequest { Username = username, LobbyId = lobbyId, ConnectionId = Context.ConnectionId };

            var response = _mediator.Send(request);
            List<string> readyPlayersName = response.Result;

            return Clients.Caller.SendAsync("PreprocessingSuccessfully", readyPlayersName);
        }

        public Task StartGame(int lobbyId)
        {
            try
            {
                var request = new StartGameRequest { LobbyId = lobbyId, ConnectionId = Context.ConnectionId };

                var response = _mediator.Send(request);
                (GameDto game, List<IPlayer> playersWithoutCaller) = response.Result;

                NotifyPlayersThatNewGameStarted(game, playersWithoutCaller);
                return Clients.Caller.SendAsync("StartGameSucceded", game);
            }
            catch (Exception)
            {
                return Clients.Caller.SendAsync("StartGameFailed", new { lobbyId });
            }
        }

        public Task ReadyToStartGame(int lobbyId, string username)
        {
            var request = new PlayerReadyRequest { Username = username, LobbyId = lobbyId, ConnectionId = Context.ConnectionId };
            var requestResult = _mediator.Send(request);

            var playersWithoutCaller = requestResult.Result;

            NotifyPlayersThatSomeone(playersWithoutCaller, "NewPlayerReady", username);
            return Clients.Caller.SendAsync("ReadySuccessfully");
        }

        public Task PlayerLeave(int lobbyId, string username, ColorType playerColor)
        {
            try
            {
                var request = new PlayerLeaveRequest { Username = username, LobbyId = lobbyId, ConnectionId = Context.ConnectionId };

                var response = _mediator.Send(request);
                var playersWithoutCaller = response.Result;

                NotifyPlayersThatSomeone(playersWithoutCaller, "PlayerLeftGame", $"{(int)playerColor}");
                return Clients.Caller.SendAsync("LeavingSucceeded");
            }
            catch (Exception)
            {
                return Clients.Caller.SendAsync("LeavingFailed");
            }
        }

        public Task RollDice(int gameId)
        {
            var request = new RollDiceRequest { GameId = gameId, ConnectionId = Context.ConnectionId };

            var response = _mediator.Send(request);
            (List<IPlayer> playersWithouCaller, int randomNumber, string nextDiceRoller, bool canMovePieces) = response.Result;

            NotifyPlayersThatANewDiceRolled(playersWithouCaller, randomNumber);
            if (nextDiceRoller != "")
            {
                NotifyPlayerThatShouldRollDice(nextDiceRoller);
            }
            return Clients.Caller.SendAsync("DiceRolled", new { diceNumber = randomNumber, canMovePieces });
        }

        public Task MovePiece(string username, int gameId, PieceDto piece, int lastDiceNumber)
        {
            var request = new PlayerMovePieceRequest
            {
                Username = username,
                GameId = gameId,
                Piece = piece,
                DiceNumber = lastDiceNumber,
                ConnectionId = Context.ConnectionId
            };

            var response = _mediator.Send(request);
            (List<PieceDto> piecesMoved, List<IPlayer> playersWithoutCaller,
                string playerWhoShouldRollDicesConnectionId, List<IPlayer> ranking) = response.Result;

            NotifyPlayersThatPiecesMoved(piecesMoved, playersWithoutCaller);
            NotifyPlayerThatShouldRollDice(playerWhoShouldRollDicesConnectionId);

            if (ranking.Count != 0)
            {
                NotifyPlayersThatGameFinished(ranking);
            }

            return Clients.Caller.SendAsync("PiecesMoved", piecesMoved);
        }

        private void NotifyPlayersThatNewGameStarted(GameDto game, List<IPlayer> players)
        {
            foreach (var player in players)
            {
                Clients.Client(player.ConnectionId).SendAsync("GameStarted", game);
            }
        }

        private void NotifyPlayersThatSomeone(List<IPlayer> players, string action, string username)
        {
            foreach (var player in players)
            {
                Clients.Client(player.ConnectionId).SendAsync(action, username);
            }
        }

        private void NotifyPlayersThatANewDiceRolled(List<IPlayer> players, int diceNumber)
        {
            foreach (var player in players)
            {
                Clients.Client(player.ConnectionId).SendAsync("DiceRolled", new { diceNumber, canMovePieces = false });
            }
        }

        private void NotifyPlayerThatShouldRollDice(string connectionId)
        {
            Clients.Client(connectionId).SendAsync("CanRollDice");
        }

        private void NotifyPlayersThatPiecesMoved(List<PieceDto> piecesMoved, List<IPlayer> players)
        {
            foreach (var player in players)
            {
                Clients.Client(player.ConnectionId).SendAsync("PiecesMoved", piecesMoved);
            }
        }

        private void NotifyPlayersThatGameFinished(List<IPlayer> ranking)
        {
            foreach (var player in ranking)
            {
                Clients.Client(player.ConnectionId).SendAsync("GameFinished", ranking);
            }
        }
    }
}