using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{
    public class Field
    {
        public int SizeX { get; init; }
        public int SizeY { get; init; }
        public IReadOnlyList<IReadOnlyList<FieldCell>> Cells { get; init; }
        public int TotalMineNum { get; init; }
        public int HiddenMineNum { get; private set; }
        public bool IsInitialized { get; private set; }

        public Field()
        {
            SizeX = 0;
            SizeY = 0;
            Cells = new List<IReadOnlyList<FieldCell>>();
            TotalMineNum = 0;
            HiddenMineNum = 0;
            IsInitialized = false;
        }

        public Field(FieldParameter param, int startX, int startY)
        {
            if (! param.IsValid())
            {
                throw new ArgumentException("Invalid field paramter");
            }

            SizeX = param.SizeX;
            SizeY = param.SizeY;
            TotalMineNum = param.TotalMineNum;
            HiddenMineNum = param.TotalMineNum;

            var isMineMap = FieldUtil.GenRandomMineMap(TotalMineNum, SizeX, SizeY, startX, startY);
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
            
            IsInitialized = true;
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
            IsInitialized = true;
        }

        public CellStatus Open(int posX, int posY)
        {
            if (! IsInitialized)
            {
                throw new ArgumentException("Not initilaized");
            }

            var openStack = new List<FieldCell>() { Cells[posY][posX] };
            return OpenCore(openStack);
        }

        public CellStatus OpenAroundOf(int posX, int posY)
        {
            if (!IsInitialized)
            {
                throw new ArgumentException("Not initilaized");
            }

            // Cannot open because center cell is not opened.
            if (Cells[posY][posX].CellStatus != CellStatus.Cleared) return CellStatus.Default;

            var flaggedCount = 0;
            if (IsFlagged(posX - 1, posY - 1)) flaggedCount++;
            if (IsFlagged(posX + 0, posY - 1)) flaggedCount++;
            if (IsFlagged(posX + 1, posY - 1)) flaggedCount++;
            if (IsFlagged(posX - 1, posY + 0)) flaggedCount++;
            if (IsFlagged(posX + 1, posY + 0)) flaggedCount++;
            if (IsFlagged(posX - 1, posY + 1)) flaggedCount++;
            if (IsFlagged(posX + 0, posY + 1)) flaggedCount++;
            if (IsFlagged(posX + 1, posY + 1)) flaggedCount++;

            // Cannot open because the number of flag is insufficient.
            if (flaggedCount != Cells[posY][posX].NeighborMineNum) return CellStatus.Default;

            // Add to openStack if status is NotOpened.
            var openStack = new List<FieldCell>();
            if (CanAroundOpen(posX - 1, posY - 1)) openStack.Add(Cells[posY - 1][posX - 1]);
            if (CanAroundOpen(posX + 0, posY - 1)) openStack.Add(Cells[posY - 1][posX + 0]);
            if (CanAroundOpen(posX + 1, posY - 1)) openStack.Add(Cells[posY - 1][posX + 1]);
            if (CanAroundOpen(posX - 1, posY + 0)) openStack.Add(Cells[posY + 0][posX - 1]);
            if (CanAroundOpen(posX + 1, posY + 0)) openStack.Add(Cells[posY + 0][posX + 1]);
            if (CanAroundOpen(posX - 1, posY + 1)) openStack.Add(Cells[posY + 1][posX - 1]);
            if (CanAroundOpen(posX + 0, posY + 1)) openStack.Add(Cells[posY + 1][posX + 0]);
            if (CanAroundOpen(posX + 1, posY + 1)) openStack.Add(Cells[posY + 1][posX + 1]);
            return OpenCore(openStack);
        }

        public void SetStatus(int posX, int posY, CellStatus cellStatus)
        {
            if (!IsInitialized)
            {
                throw new ArgumentException("Not initilaized");
            }

            if (Cells[posY][posX].CellStatus != CellStatus.Flagged && cellStatus == CellStatus.Flagged)
            {
                HiddenMineNum -= 1;
            }
            else if (Cells[posY][posX].CellStatus == CellStatus.Flagged && cellStatus != CellStatus.Flagged)
            {
                HiddenMineNum += 1;
            }
                Cells[posY][posX].CellStatus = cellStatus;
        }

        public int CountHiddenMine()
        {
            int hiddenMineNum = 0;
            for (int iy = 0; iy < SizeY; iy++)
            {
                for (int ix = 0; ix < SizeX; ix++)
                {
                    if (Cells[iy][ix].CellType != CellType.Mine) continue;
                    if (Cells[iy][ix].CellStatus == CellStatus.Flagged) continue;
                    hiddenMineNum++;
                }
            }
            return hiddenMineNum;
        }

        public bool IsEnd()
        {
            if (!IsInitialized)
            {
                return false;
            }

            for (int iy = 0; iy < SizeY; iy++)
            {
                for (int ix = 0; ix < SizeX; ix++)
                {
                    if (Cells[iy][ix].CellType == CellType.Mine) continue;
                    if (Cells[iy][ix].CellStatus != CellStatus.Cleared) return false;
                }
            }
            return true;
        }

        private bool IsInField(int posX, int posY)
        {
            if (posX < 0) return false;
            if (posX >= SizeX) return false;
            if (posY < 0) return false;
            if (posY >= SizeY) return false;
            return true;
        }

        private bool DecisionToAddToStack(int posX, int posY)
        {
            // out of field
            if (! IsInField(posX, posY)) return false;

            var cell = Cells[posY][posX];
            // danger status
            if (cell.CellStatus != CellStatus.Unopened) return false;
            return true;
        }

        private bool IsFlagged(int posX, int posY)
        {
            // out of field
            if (!IsInField(posX, posY)) return false;

            return Cells[posY][posX].CellStatus == CellStatus.Flagged;
        }

        private bool CanAroundOpen(int posX, int posY)
        {
            // out of field
            if (!IsInField(posX, posY)) return false;

            return Cells[posY][posX].CellStatus == CellStatus.Unopened;
        }

        private CellStatus OpenCore(List<FieldCell> openStack)
        {
            while (openStack.Count > 0)
            {
                var cell = openStack.Last();
                openStack.RemoveAt(openStack.Count - 1);

                var st = cell.TryOpen();
                if (st == CellStatus.Detonated)
                {
                    return CellStatus.Detonated;
                }

                if (cell.NeighborMineNum == 0)
                {
                    if (DecisionToAddToStack(cell.PosX - 1, cell.PosY - 1)) openStack.Add(Cells[cell.PosY - 1][cell.PosX - 1]);
                    if (DecisionToAddToStack(cell.PosX + 0, cell.PosY - 1)) openStack.Add(Cells[cell.PosY - 1][cell.PosX + 0]);
                    if (DecisionToAddToStack(cell.PosX + 1, cell.PosY - 1)) openStack.Add(Cells[cell.PosY - 1][cell.PosX + 1]);
                    if (DecisionToAddToStack(cell.PosX - 1, cell.PosY + 0)) openStack.Add(Cells[cell.PosY + 0][cell.PosX - 1]);
                    if (DecisionToAddToStack(cell.PosX + 1, cell.PosY + 0)) openStack.Add(Cells[cell.PosY + 0][cell.PosX + 1]);
                    if (DecisionToAddToStack(cell.PosX - 1, cell.PosY + 1)) openStack.Add(Cells[cell.PosY + 1][cell.PosX - 1]);
                    if (DecisionToAddToStack(cell.PosX + 0, cell.PosY + 1)) openStack.Add(Cells[cell.PosY + 1][cell.PosX + 0]);
                    if (DecisionToAddToStack(cell.PosX + 1, cell.PosY + 1)) openStack.Add(Cells[cell.PosY + 1][cell.PosX + 1]);
                }
            }
            return CellStatus.Cleared;
        }
    }
}
