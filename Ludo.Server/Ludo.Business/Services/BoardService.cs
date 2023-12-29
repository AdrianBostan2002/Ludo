using Ludo.Domain.Entities;
using Ludo.Domain.Enums;
using Ludo.Domain.Interfaces;

namespace Ludo.Business.Services
{
    public class BoardService : IBoardService
    {
        private readonly ICellFactory _cellFactory;

        public BoardService(ICellFactory cellFactory)
        {
            _cellFactory = cellFactory ?? throw new ArgumentNullException(nameof(cellFactory));
        }

        public Board CreateBoard()
        {
            List<ICell> cells = new List<ICell>();

            CreateSideOfBoard(cells, ColorType.Green, ColorType.Yellow);
            CreateSideOfBoard(cells, ColorType.Yellow, ColorType.Blue);
            CreateSideOfBoard(cells, ColorType.Blue, ColorType.Red);
            CreateSideOfBoard(cells, ColorType.Red, ColorType.Green);

            Board board = new Board()
            {
                Cells = cells,
            };

            return board;
        }

        private void CreateSideOfBoard(List<ICell> cells, ColorType homeColor, ColorType finalColor)
        {
            List<ICell> homeCell = CreateSetOfCells(CellType.Home, homeColor, 1);
            List<ICell> basicCells = CreateSetOfCells(CellType.Basic, ColorType.White, 10);
            List<ICell> specialCell = CreateSetOfCells(CellType.Special, finalColor, 1);
            List<ICell> basicCell = CreateSetOfCells(CellType.Basic, ColorType.White, 1);

            List<ICell> finalCells = CreateSetOfCells(CellType.Final, ColorType.White, 5);

            var specialCellEntity = specialCell.FirstOrDefault() as SpecialCell;
            specialCellEntity.FinalCells = new List<ICell>();
            specialCellEntity.FinalCells.AddRange(finalCells);

            cells.AddRange(homeCell);
            cells.AddRange(basicCells);
            cells.AddRange(specialCell);
            cells.AddRange(basicCell);
        }

        private List<ICell> CreateSetOfCells(CellType cellType, ColorType color, int numberOfCells)
        {
            List<ICell> createdCells = new List<ICell>();

            for (int i = 0; i < numberOfCells; i++)
            {
                ICell newCell = _cellFactory.CreateCell(cellType);
                newCell.Color = color;
                createdCells.Add(newCell);
            }

            return createdCells;
        }

        private List<ICell> GetFinalCells(Board board)
        {
            return  board.Cells.OfType<SpecialCell>().SelectMany(c => c.FinalCells).ToList();
        }
    }
}