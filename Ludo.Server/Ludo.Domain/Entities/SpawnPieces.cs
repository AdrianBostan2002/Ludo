using Ludo.Domain.Enums;

namespace Ludo.Domain.Entities
{
    public class SpawnPieces
    {
        public ColorType Color { get; set; }     

        public List<Piece?> Pieces { get; set; }

        public SpawnPieces()
        {   
            Pieces = new List<Piece>();
        }
    }
}