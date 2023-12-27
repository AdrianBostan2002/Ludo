using Ludo.Domain.DTOs;
using Ludo.Domain.Entities;
using Ludo.Domain.Enums;
using Ludo.Domain.Interfaces;

namespace Ludo.Business.Services
{
    public class PieceService : IPieceService
    {
        public int GREEN_START_POSITION => 0;

        public int YELLOW_START_POSITION => 13;

        public int BLUE_START_POSITION => 26;

        public int RED_START_POSITION => 39;

        public Piece CreatePiece(ColorType color)
        {
            return new Piece() { Color = color };
        }

        public List<PieceDto> AssignPiecesStartPosition(List<Piece> pieces)
        {
            var piecesDto = new List<PieceDto>();

            if (pieces.Count != 0)
            {
                var firstPiece = pieces.FirstOrDefault();

                int startPosition = GetBasePosition(firstPiece.Color);

                foreach (Piece piece in pieces)
                {
                    PieceDto pieceDto = new PieceDto
                    {
                        Color = piece.Color,
                        NextPosition = startPosition,
                    };

                    piecesDto.Add(pieceDto);
                }
            }

            return piecesDto;
        }

        public int GetBasePosition(ColorType color)
        {
            int position = 0;

            switch (color)
            {
                case ColorType.Green:
                    position = GREEN_START_POSITION;
                    break;
                case ColorType.Red:
                    position = RED_START_POSITION;
                    break;
                case ColorType.Yellow:
                    position = YELLOW_START_POSITION;
                    break;
                case ColorType.Blue:
                    position = BLUE_START_POSITION;
                    break;
                default:
                    break;
            }

            return position;
        }
    }
}