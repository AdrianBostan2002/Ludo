﻿using Ludo.Business.Extensions;
using Ludo.Domain.Entities;
using Ludo.Domain.Enums;
using Ludo.Domain.Interfaces;
using System.Collections.Immutable;

namespace Ludo.Business.Services
{
    public class GameService : IGameService
    {
        private IImmutableDictionary<int, IGame> _games = ImmutableDictionary<int, IGame>.Empty;

        private readonly IBoardService _boardService;
        private readonly IPieceService _pieceService;

        public GameService(IBoardService boardService, IPieceService pieceService)
        {
            _boardService = boardService ?? throw new ArgumentNullException(nameof(boardService));
            _pieceService = pieceService ?? throw new ArgumentNullException(nameof(pieceService));
        }

        public void CreateNewGame(ILobby lobby)
        {
            Board board = _boardService.CreateBoard();

            IGame newGame = new Game()
            {
                Id = lobby.LobbyId,
                Board = board,
                Players = new List<IPlayer>()
            };

            _games = _games.Add(lobby.LobbyId, newGame);
        }

        public void AddNewPlayerIntoGame(IGame game, string username, string connectionId)
        {
            Player player = new Player
            {
                Name = username,
                IsReady = false,
                ConnectionId = connectionId
            };

            game.Players.Add(player);
        }

        public bool RemovePlayerFromGame(int gameId, string username)
        {
            IGame game = GetGameById(gameId);

            if (game == null)
            {
                return false;
            }

            IPlayer player = game.Players.Where(p => p.Name == username).FirstOrDefault();

            if (player == null)
            {
                return false;
            }

            game.Players.Remove(player);
            return true;
        }

        private List<IPlayer> TransformLobbyParticipantsIntoPlayers(List<ILobbyParticipant> lobbyParticipants)
        {
            List<IPlayer> players = new List<IPlayer>();

            foreach (ILobbyParticipant participant in lobbyParticipants)
            {
                IPlayer player = participant.ToPlayer();
                players.Add(player);
            }

            return players;
        }

        private void AssignPlayersPiecesRandomColors(List<IPlayer> players)
        {
            List<ColorType> shuffledColors = ShuflleColorsForPieces();

            foreach (var player in players)
            {
                var pieces = new List<Piece>();

                for (int i = 0; i < 4; i++)
                {
                    Piece piece = _pieceService.CreatePiece(shuffledColors.Last());

                    pieces.Add(piece);
                }

                shuffledColors.Remove(shuffledColors.Last());

                player.Pieces = pieces;
            }
        }

        private List<ColorType> ShuflleColorsForPieces()
        {
            List<ColorType> colors = new List<ColorType>() { ColorType.Blue, ColorType.Red, ColorType.Green, ColorType.Yellow };

            var random = new Random();

            var shuffledColors = colors.OrderBy(c => random.Next()).ToList();
            return shuffledColors;
        }

        public IGame GetGameById(int id)
        {
            IGame game;

            if (_games.TryGetValue(id, out game))
            {
                return game;
            }

            return null;
        }

        public Board GetGameBoard(int gameId)
        {
            IGame game = GetGameById(gameId);

            return game?.Board;
        }

        public List<IPlayer> GetReadyPlayers(int gameId)
        {
            List<IPlayer> readyPlayers = new List<IPlayer>();

            IGame game = GetGameById(gameId);

            if (game != null)
            {
                readyPlayers = game.Players.Where(p => p.IsReady == true).ToList();
            }

            return readyPlayers;
        }

        public IPlayer GetPlayer(int gameId, string username)
        {
            IGame game = GetGameById(gameId);

            if (game == null)
            {
                return null;
            }

            return game.Players.FirstOrDefault(p => p.Name.Equals(username));
        }

        public bool CheckIfGameCanStart(int gameId)
        {
            IGame game = GetGameById(gameId);

            if (game == null)
            {
                return false;
            }

            if (!(game.Players.Count > 1 && game.Players.Count <= 4))
            {
                return false;
            }

            int playersReadyCount = game.Players.Count(p => p.IsReady == true);

            if (!(playersReadyCount == game.Players.Count - 1))
            {
                return false;
            }

            return true;
        }
    }
}