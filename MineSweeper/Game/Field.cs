using System.Collections.Generic;

namespace Game
{
    public class Field
    {
        public int SizeX { get; init; }
        public int SizeY { get; init; }
        public IReadOnlyList<IReadOnlyList<FieldCell>> Cells { get; init; }
        public int TotalMineNum { get; init; }
        public int HiddenMineNum { get; private set; }

        public Field(int sizeX, int sizeY, int totalMineNum)
        {
            SizeX = sizeX;
            SizeY = sizeY;
            TotalMineNum = totalMineNum;
            HiddenMineNum = totalMineNum;

            var isMineMap = FieldUtil.GenRandomMineMap(TotalMineNum, SizeX, SizeY);
            var neighborMineNum = FieldUtil.GenNeighborMineNumMap(in isMineMap);

            var _cells = new List<IReadOnlyList<FieldCell>>();
            for (int iy = 0; iy < SizeY; iy++)
            {
                var _cellLine = new List<FieldCell>();
                for (int ix = 0; ix < SizeX; ix++)
                {
                    CellType cellType = isMineMap[iy][ix] ? CellType.Mine : CellType.Plane;
                    var cell = new FieldCell(cellType, ix, iy, neighborMineNum[iy][ix]);
                    _cellLine.Add(cell);
                }
                _cells.Add(_cellLine);
            }
            Cells = _cells;
        }
        
        public Field(IReadOnlyList<IReadOnlyList<bool>> isMineMap)
        {
            // for debug
            SizeX = isMineMap[0].Count;
            SizeY = isMineMap.Count;

            var neighborMineNum = FieldUtil.GenNeighborMineNumMap(in isMineMap);

            var _cells = new List<IReadOnlyList<FieldCell>>();
            var _totalNum = 0;
            for (int iy = 0; iy < SizeY; iy++)
            {
                var _cellLine = new List<FieldCell>();
                for (int ix = 0; ix < SizeX; ix++)
                {
                    CellType cellType = isMineMap[iy][ix] ? CellType.Mine : CellType.Plane;
                    var cell = new FieldCell(cellType, ix, iy, neighborMineNum[iy][ix]);
                    _cellLine.Add(cell);
                    _totalNum += isMineMap[iy][ix] ? 1 : 0;
                }
                _cells.Add(_cellLine);
            }
            Cells = _cells;
            TotalMineNum = _totalNum;
            HiddenMineNum = _totalNum;
        }
    }
}
