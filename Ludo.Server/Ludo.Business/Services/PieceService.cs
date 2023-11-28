using Ludo.Domain.Entities;
using Ludo.Domain.Enums;
using Ludo.Domain.Interfaces;

namespace Ludo.Business.Services
{
    public class PieceService : IPieceService
    {
        public Piece CreatePiece(ColorType color)
        {
            return new Piece() { Color = color };
        }
    }
}