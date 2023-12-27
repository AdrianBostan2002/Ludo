using Ludo.Domain.DTOs;
using Ludo.Domain.Entities;
using Ludo.Domain.Enums;

namespace Ludo.Domain.Interfaces
{
    public interface IPieceService
    {
        int GREEN_START_POSITION { get; }
        int YELLOW_START_POSITION { get; } 
        int BLUE_START_POSITION { get; }
        int RED_START_POSITION { get; }

        Piece CreatePiece(ColorType color);

        List<PieceDto> AssignPiecesStartPosition(List<Piece> pieces);

        int GetBasePosition(ColorType color);
    }
}