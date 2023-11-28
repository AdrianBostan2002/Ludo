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
                    return new HomeCell();

                case CellType.Basic:
                    return new BasicCell();

                case CellType.Special:
                    return new SpecialCell();

                case CellType.Final:
                    return new FinalCell();
            }

            return cell;
        }
    }
}