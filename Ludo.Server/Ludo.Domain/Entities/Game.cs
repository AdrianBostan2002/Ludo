using Ludo.Domain.Interfaces;

namespace Ludo.Domain.Entities
{
    public class Game : IGame
    {
        public List<IPlayer>? Players { get; set; }
        public Board? Board { get; set; }
    }
}
