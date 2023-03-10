namespace Game
{
    public enum CellType
    {
        Default,
        Plane,
        Mine
    }

    public enum CellStatus
    {
        Default,
        Unopened,
        Cleared,
        Flagged,
        Suspicious,
        Detonated
    }

    public class FieldCell
    {
        public CellType CellType { get; init; }
        public CellStatus CellStatus { get; set; }
        public int PosX { get; init; }
        public int PosY { get; init; }
        public int NeighborMineNum { get; init; }

        public FieldCell(CellType cellType, int posX, int posY, int neighborMineNum)
        {
            CellType = cellType;
            CellStatus = CellStatus.Unopened;
            PosX = posX;
            PosY = posY;
            NeighborMineNum = neighborMineNum;
        } 

        public CellStatus TryOpen()
        {
            switch (CellStatus)
            {
                case CellStatus.Unopened:
                    if (CellType == CellType.Mine)
                    {
                        CellStatus = CellStatus.Detonated;
                        return CellStatus.Detonated;
                    }
                    else
                    {
                        CellStatus = CellStatus.Cleared;
                        return CellStatus.Cleared;
                    }
                default:
                    return CellStatus;
            }
        }
    }
}
