using Ludo.Domain.Interfaces;

namespace Ludo.Domain.DTOs
{
    public class GameDto
    {
        public int Id { get; set; }
        public List<IPlayer>? Players { get; set; }
    }
}