using Game;

namespace GameTests
{
    public class FieldUtilTests
    {
        [Fact]
        public void GenZeroFieldIntTest()
        {
            var zeroFiels = FieldUtil.GenZeroField<int>(3, 4);

            Assert.Equal(4, zeroFiels.Count);
            for (int iy = 0; iy < 4; iy++)
            {
                Assert.Equal(3, zeroFiels[iy].Count);
            }

            for (int iy = 0; iy < 4; iy++)
            {
                for (int ix = 0; ix < 3; ix++)
                {
                    Assert.Equal(0, zeroFiels[iy][ix]);
                }
            }
        }

        [Fact]
        public void GenZeroFieldBoolTest()
        {
            var zeroFiels = FieldUtil.GenZeroField<bool>(3, 4);

            for (int iy = 0; iy < 4; iy++)
            {
                for (int ix = 0; ix < 3; ix++)
                {
                    Assert.False(zeroFiels[iy][ix]);
                }
            }
        }

        [Fact]
        public void GenRandomMineMapTest()
        {
            int sizeX = 5;
            int sizeY = 8;
            var mineMap = FieldUtil.GenRandomMineMap(10, sizeX, sizeY, 0, 0);

            int totalNum = 0;
            for (int iy = 0; iy < sizeY; iy++)
            {
                for (int ix = 0; ix < sizeX; ix++)
                {
                    totalNum += mineMap[iy][ix] ? 1 : 0;
                }
            }
            Assert.Equal(10, totalNum);
        }

        [Fact]
        public void GenNeighborMineNumMapTest()
        {
            var mineMap = new List<List<bool>>
            {
                new List<bool> { false, false, true, true, false},
                new List<bool> { true, false, false, true, false},
                new List<bool> { false, false, false, false, true},
                new List<bool> { false, false, false, true, true},
            };

            var expectedMap = new List<List<int>>
            {
                new List<int> { 1, 2, 2, 2, 2 },
                new List<int> { 0, 2, 3, 3, 3 },
                new List<int> { 1, 1, 2, 4, 3 },
                new List<int> { 0, 0, 1, 2, 2 },
            };

            var actualNeigborMineNumMap = FieldUtil.GenNeighborMineNumMap((IReadOnlyList<IReadOnlyList<bool>>)mineMap);

            for (int iy = 0; iy < 4; iy++)
            {
                for (int ix = 0; ix < 5; ix++)
                {
                    Assert.Equal(expectedMap[iy][ix], actualNeigborMineNumMap[iy][ix]);
                }
            }
        }

        [Fact]
        public void TooManyMineTest()
        {
            var ex = Assert.Throws<ArgumentException>(() => FieldUtil.GenRandomMineMap(99999, 4, 6, 0, 0));
            Assert.Contains("Too many mines.", ex.Message);
        }
    }
}