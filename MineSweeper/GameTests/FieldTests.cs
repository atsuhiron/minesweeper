﻿using Game;

namespace GameTests
{
    public class FieldTests
    {
        [Fact]
        public void OpenPlaneFieldTest()
        {
            // * *
            // * *
            //
            // ↓ open (0, 0)
            //
            // 0 0
            // 0 0

            var mineMap = new List<List<bool>>
            {
                new List<bool>() { false, false },
                new List<bool>() { false, false }
            };

            var field = new Field(mineMap);
            var st = field.Open(0, 0);

            Assert.Equal(CellStatus.Cleared, st);
            for (int iy = 0; iy < 2; iy++)
            {
                for (int ix = 0; ix < 2; ix++)
                {
                    Assert.Equal(CellStatus.Cleared, field.Cells[iy][ix].CellStatus);
                }
            }
        }

        [Fact]
        public void OpenDetonatedTest()
        {
            // M *
            // * *
            //
            // ↓ open (0, 0)
            //
            // D N
            // N N

            var mineMap = new List<List<bool>>
            {
                new List<bool>() { true, false },
                new List<bool>() { false, false }
            };

            var field = new Field(mineMap);
            var st = field.Open(0, 0);

            Assert.Equal(CellStatus.Detonated, st);
            Assert.Equal(CellStatus.Detonated, field.Cells[0][0].CellStatus);
            Assert.Equal(CellStatus.NotOpened, field.Cells[0][1].CellStatus);
            Assert.Equal(CellStatus.NotOpened, field.Cells[1][0].CellStatus);
            Assert.Equal(CellStatus.NotOpened, field.Cells[1][1].CellStatus);
        }

        [Fact]
        public void OpenAroundMineTest()
        {
            // M *
            // * *
            //
            // ↓ open (1, 1)
            //
            // N N
            // N 1

            var mineMap = new List<List<bool>>
            {
                new List<bool>() { true, false },
                new List<bool>() { false, false }
            };

            var field = new Field(mineMap);
            var st = field.Open(1, 1);

            Assert.Equal(CellStatus.Cleared, st);
            Assert.Equal(CellStatus.NotOpened, field.Cells[0][0].CellStatus);
            Assert.Equal(CellStatus.NotOpened, field.Cells[0][1].CellStatus);
            Assert.Equal(CellStatus.NotOpened, field.Cells[1][0].CellStatus);
            Assert.Equal(CellStatus.Cleared, field.Cells[1][1].CellStatus);
        }

        [Fact]
        public void OpenFarFromMineTest()
        {
            // M * *
            // * * *
            // * * *
            // 
            // ↓ open (2, 2)
            //
            // N 1 0
            // 1 1 0
            // 0 0 0

            var mineMap = new List<List<bool>>
            {
                new List<bool>() { true, false, false },
                new List<bool>() { false, false, false },
                new List<bool>() { false, false, false }
            };

            var field = new Field(mineMap);
            var st = field.Open(2, 2);

            Assert.Equal(CellStatus.Cleared, st);
            Assert.Equal(CellStatus.NotOpened, field.Cells[0][0].CellStatus);
            Assert.Equal(CellStatus.Cleared, field.Cells[0][1].CellStatus);
            Assert.Equal(CellStatus.Cleared, field.Cells[1][0].CellStatus);
            Assert.Equal(CellStatus.Cleared, field.Cells[1][1].CellStatus);
            Assert.Equal(CellStatus.Cleared, field.Cells[0][2].CellStatus);
            Assert.Equal(CellStatus.Cleared, field.Cells[1][2].CellStatus);
            Assert.Equal(CellStatus.Cleared, field.Cells[2][0].CellStatus);
            Assert.Equal(CellStatus.Cleared, field.Cells[2][1].CellStatus);
            Assert.Equal(CellStatus.Cleared, field.Cells[2][2].CellStatus);
        }

        [Fact]
        public void OpenFarFromMinePreviouslyOpenedTest()
        {
            // M * *
            // * * *
            // * * *
            // 
            // ↓ init (open (1, 0))
            //
            // N 1 N
            // N N N
            // N N N
            // 
            // ↓ open (2, 2)
            //
            // N 1 0
            // 1 1 0
            // 0 0 0

            var mineMap = new List<List<bool>>
            {
                new List<bool>() { true, false, false },
                new List<bool>() { false, false, false },
                new List<bool>() { false, false, false }
            };

            var field = new Field(mineMap);
            _ = field.Open(1, 0);
            var st = field.Open(2, 2);

            Assert.Equal(CellStatus.Cleared, st);
            Assert.Equal(CellStatus.NotOpened, field.Cells[0][0].CellStatus);
            Assert.Equal(CellStatus.Cleared, field.Cells[0][1].CellStatus);
            Assert.Equal(CellStatus.Cleared, field.Cells[1][0].CellStatus);
            Assert.Equal(CellStatus.Cleared, field.Cells[1][1].CellStatus);
            Assert.Equal(CellStatus.Cleared, field.Cells[0][2].CellStatus);
            Assert.Equal(CellStatus.Cleared, field.Cells[1][2].CellStatus);
            Assert.Equal(CellStatus.Cleared, field.Cells[2][0].CellStatus);
            Assert.Equal(CellStatus.Cleared, field.Cells[2][1].CellStatus);
            Assert.Equal(CellStatus.Cleared, field.Cells[2][2].CellStatus);
        }

        [Fact]
        public void OpenWithoutFlaggedTest()
        {
            // * * *
            // F * *
            // * * *
            // 
            // ↓ open (2, 2)
            //
            // 0 0 0
            // N 0 0
            // 0 0 0

            var mineMap = new List<List<bool>>
            {
                new List<bool>() { false, false, false },
                new List<bool>() { false, false, false },
                new List<bool>() { false, false, false }
            };

            var field = new Field(mineMap);
            field.SetStatus(0, 1, CellStatus.Flagged);
            var st = field.Open(2, 2);

            Assert.Equal(CellStatus.Cleared, st);
            Assert.Equal(CellStatus.Cleared, field.Cells[0][0].CellStatus);
            Assert.Equal(CellStatus.Cleared, field.Cells[0][1].CellStatus);
            Assert.Equal(CellStatus.Flagged, field.Cells[1][0].CellStatus);
            Assert.Equal(CellStatus.Cleared, field.Cells[1][1].CellStatus);
            Assert.Equal(CellStatus.Cleared, field.Cells[0][2].CellStatus);
            Assert.Equal(CellStatus.Cleared, field.Cells[1][2].CellStatus);
            Assert.Equal(CellStatus.Cleared, field.Cells[2][0].CellStatus);
            Assert.Equal(CellStatus.Cleared, field.Cells[2][1].CellStatus);
            Assert.Equal(CellStatus.Cleared, field.Cells[2][2].CellStatus);
        }

        [Fact]
        public void CountHiddenMinetest1()
        {
            var mineMap = new List<List<bool>>
            {
                new List<bool>() { false, false, false },
                new List<bool>() { false, false, false },
                new List<bool>() { false, false, false }
            };

            var field = new Field(mineMap);
            Assert.Equal(0, field.CountHiddenMine());

            field.Open(1, 1);
            Assert.Equal(0, field.CountHiddenMine());
        }

        [Fact]
        public void CountHiddenMinetest2()
        {
            var mineMap = new List<List<bool>>
            {
                new List<bool>() { false, false, false },
                new List<bool>() { true, false, false },
                new List<bool>() { false, false, false }
            };

            var field = new Field(mineMap);
            Assert.Equal(1, field.CountHiddenMine());

            _ = field.Open(1, 1);
            Assert.Equal(1, field.CountHiddenMine());

            field.SetStatus(0, 1, CellStatus.Flagged);
            Assert.Equal(0, field.CountHiddenMine());
        }

        [Fact]
        public void CountHiddenMinetest3()
        {
            var mineMap = new List<List<bool>>
            {
                new List<bool>() { false, false, false },
                new List<bool>() { true, false, true },
                new List<bool>() { false, false, false }
            };

            var field = new Field(mineMap);
            Assert.Equal(2, field.CountHiddenMine());

            _ = field.Open(1, 1);
            Assert.Equal(2, field.CountHiddenMine());

            field.SetStatus(0, 1, CellStatus.Flagged);
            field.SetStatus(2, 1, CellStatus.Suspicious);
            Assert.Equal(1, field.CountHiddenMine());
        }

        [Fact]
        public void IsEndTest1()
        {
            var mineMap = new List<List<bool>>
            {
                new List<bool>() { false, false, false },
                new List<bool>() { false, false, false },
                new List<bool>() { false, false, false }
            };

            var field = new Field(mineMap);
            // * * *
            // * * *
            // * * *
            Assert.False(field.IsEnd());
            
            _ = field.Open(0, 0);
            // 0 0 0
            // 0 0 0
            // 0 0 0
            Assert.True(field.IsEnd());
        }

        [Fact]
        public void IsEndTest2()
        {
            var mineMap = new List<List<bool>>
            {
                new List<bool>() { true, false, false },
                new List<bool>() { false, false, false },
                new List<bool>() { false, false, false }
            };

            var field = new Field(mineMap);
            // M * *
            // * * *
            // * * *
            Assert.False(field.IsEnd());

            _ = field.Open(1, 1);
            // N N N
            // N 1 N
            // N N N
            Assert.False(field.IsEnd());

            _ = field.Open(2, 2);
            // N 1 0
            // 1 1 0
            // 0 0 0
            Assert.True(field.IsEnd());
        }

        [Fact]
        public void IsEndTest3()
        {
            var mineMap = new List<List<bool>>
            {
                new List<bool>() { true, true, false },
                new List<bool>() { false, false, false },
                new List<bool>() { false, false, false }
            };

            var field = new Field(mineMap);
            // M M *
            // * * *
            // * * *
            field.SetStatus(0, 0, CellStatus.Flagged);
            // F N N
            // N N N
            // N N N
            field.SetStatus(1, 0, CellStatus.Suspicious);
            // F S N
            // N N N
            // N N N
            Assert.False(field.IsEnd());

            _ = field.Open(2, 2);
            // F S N
            // 2 2 1
            // 0 0 0
            Assert.False(field.IsEnd());

            _ = field.Open(2, 0);
            // F S 1
            // 2 2 1
            // 0 0 0
            Assert.True(field.IsEnd());
        }
    }
}
