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
    }
}