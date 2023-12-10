using Ludo.Domain.Enums;
using Ludo.Domain.Interfaces;

namespace Ludo.Domain.Entities
{
    public class SpecialCell : ICell, IFinalCells
    {
        public List<Piece> Pieces { get; set; }

        public ColorType Color { get; set; }

        public List<ICell> FinalCells { get; set; }

        private CellType _type;
        public CellType Type { get { return _type; } }

        public SpecialCell()
        {
            _type = CellType.Special;
        }
    }
}