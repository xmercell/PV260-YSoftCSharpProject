using StockGrader.BL.Model;

namespace StockGrader.BL.Test.PositionsTest
{
    public class PositionsTests
    {
        [Test]
        public void TestPositionToString()
        {
            new When()
                .BasicPosition()
                .AssertPositionString();
        }

        [Test]
        public void TestRemovedPositionToString()
        {
            new When()
                .RemovedPosition() 
                .AssertPositionString();
        }

        [Test]
        public void TestIncreasedPositionToString()
        {
            new When()
                .IncreasedPosition()
                .AssertPositionString();
        }

        [Test]
        public void TestDecreasedPositionToString()
        {
            new When()
                .DecreasedPosition()
                .AssertPositionString();
        }
    }
}