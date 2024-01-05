using Ludo.Business.Options;
using Ludo.Domain.DTOs;
using Ludo.Domain.Entities;
using Ludo.Domain.Enums;
using Ludo.Domain.Interfaces;
using Microsoft.Extensions.Options;

namespace Ludo.Business.Services
{
    public class PieceService : IPieceService
    {
        private readonly LudoGameOptions _options;

        public PieceService(IOptions<LudoGameOptions> options)
        {
            _options = options.Value ?? throw new ArgumentNullException(nameof(_options));
        }

        public Piece CreatePiece(ColorType color)
        {
            return new Piece() { Color = color };
        }

        public List<PieceDto> AssignPiecesStartPosition(List<Piece> pieces)
        {
            var piecesDto = new List<PieceDto>();

            for (int i = 0; i < pieces.Count; i++)
            {
                int startPosition = GetSpawnPosition(pieces[i].Color, i);

                PieceDto pieceDto = new PieceDto
                {
                    Color = pieces[i].Color,
                    NextPosition = startPosition,
                };

                piecesDto.Add(pieceDto);
            }

            return piecesDto;
        }

        public int GetSpawnPosition(ColorType color, int index)
        {
            int colorIndex = (int)color;
            string position = $"{colorIndex + 5}{colorIndex}{index}";

            return int.Parse(position);
        }

        public int GetBasePosition(ColorType color)
        {
            int position = 0;

            switch (color)
            {
                case ColorType.Green:
                    position = _options.GreenStartPosition;
                    break;
                case ColorType.Red:
                    position = _options.RedStartPosition;
                    break;
                case ColorType.Yellow:
                    position = _options.YellowStartPosition;
                    break;
                case ColorType.Blue:
                    position = _options.BlueStartPosition;
                    break;
                default:
                    break;
            }

            return position;
        }
    }
}