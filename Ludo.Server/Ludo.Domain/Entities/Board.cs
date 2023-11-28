using Ludo.Domain.Interfaces;

namespace Ludo.Domain.Entities
{
    public class Board
    {
        public List<ICell> Cells { get; set; }
    }
}