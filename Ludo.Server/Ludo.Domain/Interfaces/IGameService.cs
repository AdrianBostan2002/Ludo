using Ludo.Domain.Entities;

namespace Ludo.Domain.Interfaces
{
    public interface IGameService
    {
        void CreateNewGame(ILobby lobby);
        IGame GetGameById(int id);
        Board GetGameBoard(int gameId);
    }
}