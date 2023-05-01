using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockGrader.BL.Model;

namespace StockGrader.BL.Test.PositionsTest
{
    public class When
    {
        IPosition currentPosition;
        string expectedPositionString;

        public When BasicPosition()
        {
            currentPosition = new Position("Microsoft", "MSFT", 100, 0.5);
            expectedPositionString = $"Microsoft, MSFT, 100, {0.5}(%)";
            return this;
        }

        public When RemovedPosition()
        {
            currentPosition = new RemovedPosition("Microsoft", "MSFT");
            expectedPositionString = "Microsoft, MSFT";
            return this;
        }

        public When IncreasedPosition()
        {
            currentPosition = new UpdatedPosition("Microsoft", "MSFT", 100, 0.5, 10);
            expectedPositionString = $"Microsoft, MSFT, 100( 🔺 10%), {0.5}(%)";
            return this;
        }
        
        public When DecreasedPosition()
        {
            currentPosition = new UpdatedPosition("Microsoft", "MSFT", 100, 0.5, -10);
            expectedPositionString = $"Microsoft, MSFT, 100( 🔻 10%), {0.5}(%)";
            return this;
        }

        public void AssertPositionString()
        {
            var position = new Position("Microsoft", "MSFT", 100, 0.5);
            Assert.That(currentPosition.ToString(), Is.EqualTo(expectedPositionString));
        }
    }
}
