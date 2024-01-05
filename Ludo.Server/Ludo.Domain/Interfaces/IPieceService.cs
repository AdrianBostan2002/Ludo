using Ludo.Domain.DTOs;
using Ludo.Domain.Entities;
using Ludo.Domain.Enums;

namespace Ludo.Domain.Interfaces
{
    public interface IPieceService
    {
        Piece CreatePiece(ColorType color);

        List<PieceDto> AssignPiecesStartPosition(List<Piece> pieces);

        int GetSpawnPosition(ColorType color, int index);

        int GetBasePosition(ColorType color);
    }
}