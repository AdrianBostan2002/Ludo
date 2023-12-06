using Ludo.Domain.Entities;
using Ludo.Domain.Enums;

namespace Ludo.Domain.Interfaces
{
    public interface IPieceService
    {
        Piece CreatePiece(ColorType color);
    }
}