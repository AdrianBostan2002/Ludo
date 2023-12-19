using Ludo.Domain.Entities;
using Ludo.Domain.Enums;

namespace Ludo.Domain.Interfaces
{
    public interface ICell
    {
        List<Piece> Pieces { get; set; }

        ColorType Color { get; set; }

        CellType Type { get; }
    }
}