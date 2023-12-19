using Ludo.Domain.Enums;
using Ludo.Domain.Interfaces;

namespace Ludo.Domain.Entities
{
    public class BasicCell : ICell
    {
        public List<Piece> Pieces { get ; set ; }

        public ColorType Color { get; set; }

        private CellType _type;
        public CellType Type { get { return _type; } }

        public BasicCell()
        {
            _type = CellType.Basic;
        }
    }
}