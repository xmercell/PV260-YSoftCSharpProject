
namespace StockGrader.BL.Test.ConsoleWriterTest
{
    public class ConsoleWriterTests
    {
        [Test]
        public void Write_ShouldWriteCorrectOutput()
        {
            new When()
                .DiffWriter()
                .AssertWriterResultCorrect();
        }

        [Test]
        public void IncreasedPositions_ShouldReturnCorrectString()
        {
            new When()
                .IncreasedPositionsBasicWriter()
                .AssertWriterResultCorrect();
        }

        [Test]
        public void NewPositions_ShouldReturnCorrectString()
        {
            new When()
                .NewPositionsBasicWriter()
                .AssertWriterResultCorrect();
        }

        [Test]
        public void ReducedPositions_ShouldReturnCorrectString()
        {
            new When()
                .ReducedPositionsBasicWriter()
                .AssertWriterResultCorrect();
        }

        [Test]
        public void RemovedPositions_ShouldReturnCorrectString()
        {
            new When()
                .RemovedPositionsBasicWriter()
                .AssertWriterResultCorrect();
        }

        [Test]
        public void UnchangedPositions_ShouldReturnCorrectString()
        {
            new When()
                .UnchangedPositionsBasicWriter()
                .AssertWriterResultCorrect();
        }

        [Test]
        public void IncreseadPositionTest()
        {
            new When()
                .IncreasedPositionWriter()
                .AssertWriterResultCorrect();
        }


        [Test]
        public void RemovedPositionTest()
        {
            new When()
                .RemovedPositionWriter()
                .AssertWriterResultCorrect();
        }


        [Test]
        public void ReducedPositionTest()
        {
            new When()
                .ReducedPositionWriter()
                .AssertWriterResultCorrect();
        }

        [Test]
        public void NewPositionTest()
        {
            new When()
                .NewPositionWriter()
                .AssertWriterResultCorrect();
        }

        [Test]
        public void UnchangedPositionTest()
        {
            new When()
                .UnchangedPositionWriter()
                .AssertWriterResultCorrect();
        }


    }
}