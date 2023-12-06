using Ludo.Domain.Enums;
using Ludo.Domain.Interfaces;

namespace Ludo.Domain.Entities
{
    public class CellFactory : ICellFactory
    {
        public ICell CreateCell(CellType cellType)
        {
            ICell cell = null;

            switch (cellType)
            {
                case CellType.Home:
                    cell = new HomeCell() { };
                    break;

                case CellType.Basic:
                    cell = new BasicCell();
                    break;

                case CellType.Special:
                    cell = new SpecialCell();
                    break;

                case CellType.Final:
                    cell = new FinalCell();
                    break;
            }

            cell.Pieces = cell != null ? new List<Piece>() : null;

            return cell;
        }
    }
}