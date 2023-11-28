using Ludo.Domain.Interfaces;

namespace Ludo.Domain.Entities
{
    public class Player : IPlayer
    {
        public string? Name { get; set; }
        public string? ConnectionId { get; set; }
        public List<Piece>? Pieces { get; set; }
    }
}
