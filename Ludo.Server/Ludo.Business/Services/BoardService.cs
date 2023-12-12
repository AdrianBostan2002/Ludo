using Ludo.Domain.Entities;
using Ludo.Domain.Enums;
using Ludo.Domain.Interfaces;
using Newtonsoft.Json.Bson;

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
            List<List<ICell>> cells = new List<List<ICell>>();

            CreateSideOfBoard(cells, ColorType.Red, ColorType.Green);
            CreateSideOfBoard(cells, ColorType.Green, ColorType.Yellow);
            CreateSideOfBoard(cells, ColorType.Yellow, ColorType.Blue);
            CreateSideOfBoard(cells, ColorType.Blue, ColorType.Red);

            Board board = new Board()
            {
                Cells = cells,
            };

            List<ICell> finalCells = GetFinalCells(board);

            board.FinalCells = finalCells;

            return board;
        }

        private void CreateSideOfBoard(List<List<ICell>> cells, ColorType homeColor, ColorType finalColor)
        {
            List<ICell> homeCell = CreateSetOfCells(CellType.Home, homeColor, 1);
            List<ICell> basicCells = CreateSetOfCells(CellType.Basic, ColorType.White, 11);
            List<ICell> specialCell = CreateSetOfCells(CellType.Special, finalColor, 1);
            List<ICell> basicCell = CreateSetOfCells(CellType.Basic, ColorType.White, 1);

            List<ICell> finalCells = CreateSetOfCells(CellType.Final, ColorType.White, 5);

            var specialCellEntity = specialCell.FirstOrDefault() as SpecialCell;
            specialCellEntity.FinalCells = new List<ICell>();
            specialCellEntity.FinalCells.AddRange(finalCells);

            var homeCellInitialized = InitializeFinalCells(homeCell);
            var basicCellInitialized = InitializeFinalCells(basicCells);
            var specialCellInitialized = InitializeFinalCells(specialCell);
            var basicOneCellInitialized = InitializeFinalCells(basicCell);

            specialCellInitialized[1].AddRange(finalCells);

            cells.AddRange(homeCellInitialized);
            cells.AddRange(basicCellInitialized);
            cells.AddRange(specialCellInitialized);
            cells.AddRange(basicOneCellInitialized);
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

        private List<List<ICell>> InitializeFinalCells(List<ICell> cells)
        {
            var boardCells = new List<List<ICell>>
            {
                cells,
                new List<ICell>()
            };

            return boardCells;
        }

        //private void AddFinalCells(List<List<ICell>> cells, List<ICell> finalCells)
        //{
        //    finalCells.ad
        //}
    }
}