using Ludo.Domain.Entities;

namespace Ludo.Domain.Interfaces
{
    public interface IGameService
    {
        void CreateNewGame(ILobby lobby);
        IGame GetGameById(int id);
        void AddNewPlayerIntoGame(IGame game, string username, string connectionId);
        Board GetGameBoard(int gameId);
        List<IPlayer> GetReadyPlayers(int gameId);
        IPlayer GetPlayer(int gameId, string username);
        bool CheckIfGameCanStart(int gameId);
        bool RemovePlayerFromGame(int gameId, string username);
        void AssignPlayersPiecesRandomColors(List<IPlayer> players);
        List<IPlayer> GetPlayersWithoutCaller(IGame game, string connectionId);
        void AssignRandomOrderForRollingDice(IGame game);
        bool CheckIfPlayerPiecesAreOnSpawnPosition(IGame game, IPlayer player);
        bool CheckIfPlayerPicesAreOnTriangleCell(IPlayer player);
        void PieceMovedOnWinningCell(IGame game, IPlayer player);
        bool CheckIfGameIsFinished(IGame game, IPlayer player);
        string GetNextDiceRoller(IGame game);
        int GetRandomDiceNumber();
    }
}