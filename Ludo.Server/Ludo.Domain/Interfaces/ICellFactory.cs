using Ludo.Domain.Enums;

namespace Ludo.Domain.Interfaces
{
    public interface ICellFactory
    {
        ICell CreateCell(CellType cellType);
    }
}