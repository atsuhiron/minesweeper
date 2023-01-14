using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{
    public static class FieldUtil
    {
        public static List<List<T>> GenZeroField<T>(int sizeX, int sizeY) where T : struct
        {
            return Enumerable.Range(0, sizeY).Select(_ => Enumerable.Range(0, sizeX).Select(_ => default(T)).ToList()).ToList();
        }


        public static IReadOnlyList<IReadOnlyList<bool>> GenRandomMineMap(int num, int sizeX, int sizeY, int sanctuaryX, int sanctuaryY)
        {
            if (num >= (sizeX * sizeY) - 1)
            {
                throw new ArgumentException($"Too many mines. Cell num is {sizeX * sizeY - 1}, but mine num is {num}.");
            }

            var rand = new Random();
            var isMineField = GenZeroField<bool>(sizeX, sizeY);

            var count = 0;
            while (count < num)
            {
                var mineX = rand.Next(sizeX);
                var mineY = rand.Next(sizeY);

                if (isMineField[mineY][mineX]) continue;
                if ((mineX == sanctuaryX) && (mineY == sanctuaryY)) continue;
                isMineField[mineY][mineX] = true;
                count++;
            }
            return isMineField;
        }

        public static IReadOnlyList<IReadOnlyList<int>> GenNeighborMineNumMap(in IReadOnlyList<IReadOnlyList<bool>> isMineMap)
        {
            var sizeX = isMineMap[0].Count;
            var sizeY = isMineMap.Count;

            var topShift = GenZeroField<int>(sizeX, sizeY);
            var bottomShift = GenZeroField<int>(sizeX, sizeY);
            for (int iy = 0; iy < sizeY - 1; iy++)
            {
                topShift[iy] = isMineMap[iy + 1].Select(x => x ? 1 : 0).ToList();
                bottomShift[iy + 1] = isMineMap[iy].Select(x => x ? 1 : 0).ToList();
            }

            var leftShift = GenZeroField<int>(sizeX, sizeY);
            var rightShift = GenZeroField<int>(sizeX, sizeY);
            for (int ix = 0; ix < sizeX - 1; ix++)
            {
                for (int iy = 0; iy < sizeY; iy++)
                {
                    leftShift[iy][ix] = isMineMap[iy][ix + 1] ? 1 : 0;
                    rightShift[iy][ix + 1] = isMineMap[iy][ix] ? 1 : 0;
                }
            }

            var topLeftShift = GenZeroField<int>(sizeX, sizeY);
            var topRightShift = GenZeroField<int>(sizeX, sizeY);
            var bottomLeftShift = GenZeroField<int>(sizeX, sizeY);
            var bottomRightShift = GenZeroField<int>(sizeX, sizeY);
            for (int iy = 0; iy < sizeY - 1; iy++)
            {
                topLeftShift[iy] = leftShift[iy + 1];
                bottomLeftShift[iy + 1] = leftShift[iy];
                topRightShift[iy] = rightShift[iy + 1];
                bottomRightShift[iy + 1] = rightShift[iy];
            }

            var neighborsMineNum = GenZeroField<int>(sizeX, sizeY);
            for (int ix = 0; ix < sizeX; ix++)
            {
                for (int iy = 0; iy < sizeY; iy++)
                {
                    neighborsMineNum[iy][ix] = topShift[iy][ix] + bottomShift[iy][ix] + leftShift[iy][ix] + rightShift[iy][ix]
                                             + topLeftShift[iy][ix] + topRightShift[iy][ix] + bottomLeftShift[iy][ix] + bottomRightShift[iy][ix];
                }
            }
            return neighborsMineNum;
        }
    }
}
