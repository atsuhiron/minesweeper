using Game;

namespace GameTests
{
    public class FieldParameterTests
    {
        [Theory]
        [InlineData(0, 0, 0, false)]
        [InlineData(0, 0, 1, false)]
        [InlineData(0, 1, 0, false)]
        [InlineData(1, 0, 0, false)]
        [InlineData(1, 0, 1, false)]
        [InlineData(1, 1, 0, false)]
        [InlineData(0, 1, 1, false)]
        [InlineData(1, 1, 1, true)]
        public void FieldParameterIsValidTest(int sizeX, int sizeY, int totalMineNum, bool expected)
        {
            var param = new FieldParameter(sizeX, sizeY, totalMineNum);
            Assert.Equal(expected, param.IsValid());
        } 
    }
}
