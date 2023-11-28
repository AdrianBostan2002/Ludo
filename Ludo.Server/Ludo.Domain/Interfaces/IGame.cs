using Ludo.Domain.Entities;

namespace Ludo.Domain.Interfaces
{
    public interface IGame
    {
        Board? Board { get; set; }
        List<IPlayer>? Players { get; set; }
    }
}