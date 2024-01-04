using Ludo.Domain.Entities;

namespace Ludo.Domain.Interfaces
{
    public interface IGame
    {
        int Id { get; set; }
        Board? Board { get; set; }
        List<IPlayer>? Players { get; set; }
        LinkedList<string> RollDiceOrder { get; set; }
        List<IPlayer> Ranking { get; set; }
    }
}