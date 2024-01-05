using Ludo.Domain.Interfaces;

namespace Ludo.Domain.Entities
{
    public class Player : IPlayer
    {
        public string? Name { get; set; }
        public string? ConnectionId { get; set; }
        public List<Piece>? Pieces { get; set; }
        public bool IsReady { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj is not Player player)
                return false;

            return IsReady == player.IsReady &&
                   Name == player.Name &&
                   ConnectionId == player.ConnectionId;
        }
    }
}
