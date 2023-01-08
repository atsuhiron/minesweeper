namespace Game
{
    public enum CellType
    {
        Default,
        Plane,
        Mine
    }

    public enum ExplorationStatus
    {
        Default,
        NotExplored,
        Cleared,
        Suspicious,
        Detonated
    }

    public class FieldCell
    {
        public CellType CellType { get; init; }
        public ExplorationStatus ExpStatus { get; set; }
        public int PosX { get; init; }
        public int PosY { get; init; }
        public int NeighborMineNum { get; init; }

        public FieldCell(CellType cellType, int posX, int posY, int neighborMineNum)
        {
            CellType = cellType;
            ExpStatus = ExplorationStatus.NotExplored;
            PosX = posX;
            PosY = posY;
            NeighborMineNum = neighborMineNum;
        } 

        public ExplorationStatus TryExplore()
        {
            switch (ExpStatus)
            {
                case ExplorationStatus.NotExplored:
                    if (CellType == CellType.Mine)
                    {
                        ExpStatus = ExplorationStatus.Detonated;
                        return ExplorationStatus.Detonated;
                    }
                    else
                    {
                        ExpStatus = ExplorationStatus.Cleared;
                        return ExplorationStatus.Cleared;
                    }
                default:
                    return ExpStatus;
            }
        }
    }
}
