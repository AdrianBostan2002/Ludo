using Ludo.Domain.Enums;

namespace Ludo.Domain.DTOs
{
    public class PieceDto
    {
        public ColorType Color { get; set; }    

        public int NextPosition { get; set; }

        public int? PreviousPosition { get; set; }   
    }
}