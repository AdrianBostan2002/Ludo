using Ludo.Domain.Interfaces;

namespace Ludo.Domain.Entities
{
    public class Board
    {
        public List<List<ICell>> Cells { get; set; }
        public List<ICell> FinalCells { get; set; }
    }
}