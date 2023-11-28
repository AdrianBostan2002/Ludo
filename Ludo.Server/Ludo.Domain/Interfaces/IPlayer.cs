using Ludo.Domain.Entities;

namespace Ludo.Domain.Interfaces
{
    public interface IPlayer
    {
        string? Name { get; set; }
        string? ConnectionId { get; set; }
        List<Piece>? Pieces { get; set; }
    }
}