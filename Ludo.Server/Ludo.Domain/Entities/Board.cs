using Ludo.Domain.Interfaces;

namespace Ludo.Domain.Entities
{
    public class Board
    {
        public List<ICell> Cells { get; set; }
        public List<List<Piece>> SpawnPieces {  get; set; }
    }
}