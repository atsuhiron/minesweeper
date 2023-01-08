using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class Field
    {
        public int SizeX { get; init; }
        public int SizeY { get; init; }
        public IReadOnlyList<IReadOnlyList<FieldCell>> Cells { get; init; }
        public int TotalMineNum { get; init; }
        public int HiddenMineNum { get; set; }

        public Field(int sizeX, int sizeY, int totalMineNum)
        {
            SizeX = sizeX;
            SizeY = sizeY;
            TotalMineNum = totalMineNum;
            HiddenMineNum = totalMineNum;

            var isMineField = FieldUtil.GenRandomMineMap(TotalMineNum, SizeX, SizeY);

            var _cells = new List<IReadOnlyList<FieldCell>>();
            for (int iy = 0; iy < SizeY; iy++)
            {
                var _cellLine = new List<FieldCell>();
                for (int ix = 0; ix < SizeX; ix++)
                {
                    CellType cellType = isMineField[iy][ix] ? CellType.Mine : CellType.Plane;
                    var cell = new FieldCell(cellType, ix, iy, -1);
                    _cellLine.Add(cell);
                }
                _cells.Add(_cellLine);
            }
            Cells = _cells;
        }        
    }
}
