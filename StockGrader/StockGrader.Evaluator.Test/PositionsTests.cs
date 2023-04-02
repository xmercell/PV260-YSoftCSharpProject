using StockGrader.Evaluator.Model;

namespace StockGrader.Evaluator.Test
{
    public class PositionsTests
    {

        [Test]
        public void TestPositionToString()
        {
            var position = new Position("Microsoft", "MSFT", 100, 0.5);
            Assert.That(position.ToString(), Is.EqualTo("Microsoft, MSFT, 100, 0,5"));

        }
        [Test]
        public void TestRemovedPositionToString()
        {
            var removedPosition = new RemovedPosition("Microsoft", "MSFT");
            Assert.That(removedPosition.ToString(), Is.EqualTo("Microsoft, MSFT"));
        }
        [Test]
        public void TestIncreasedPositionToString()
        {
            var updatedPosition = new UpdatedPosition("Microsoft", "MSFT", 100, 0.5, 10);
            Assert.That(updatedPosition.ToString(), Is.EqualTo("Microsoft, MSFT, 100(🔺10%), 0,5"));
        }
        [Test]
        public void TestDecreasedPositionToString()
        {
            var updatedPosition = new UpdatedPosition("Microsoft", "MSFT", 100, 0.5, -10);
            Assert.That(updatedPosition.ToString(), Is.EqualTo("Microsoft, MSFT, 100(🔻10%), 0,5"));
        }
    }
}