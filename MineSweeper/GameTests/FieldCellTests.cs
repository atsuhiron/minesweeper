using Game;

namespace GameTests
{
    public class FieldCellTests
    {
        [Theory]
        [InlineData(CellType.Plane, CellStatus.NotOpened, CellStatus.Cleared)]
        [InlineData(CellType.Plane, CellStatus.Cleared, CellStatus.Cleared)]
        [InlineData(CellType.Plane, CellStatus.Flagged, CellStatus.Flagged)]
        [InlineData(CellType.Plane, CellStatus.Suspicious, CellStatus.Suspicious)]
        [InlineData(CellType.Plane, CellStatus.Detonated, CellStatus.Detonated)]
        [InlineData(CellType.Mine, CellStatus.NotOpened, CellStatus.Detonated)]
        [InlineData(CellType.Mine, CellStatus.Cleared, CellStatus.Cleared)]
        [InlineData(CellType.Mine, CellStatus.Flagged, CellStatus.Flagged)]
        [InlineData(CellType.Mine, CellStatus.Suspicious, CellStatus.Suspicious)]
        [InlineData(CellType.Mine, CellStatus.Detonated, CellStatus.Detonated)]
        public void TryOpenTest(CellType cellType, CellStatus initStatus, CellStatus expectedRestltStatus)
        {
            var cell = new FieldCell(cellType, 0, 0, 0)
            {
                CellStatus = initStatus
            };

            Assert.Equal(expectedRestltStatus, cell.TryOpen());
        }
    }
}
