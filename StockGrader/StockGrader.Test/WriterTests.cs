using Moq;
using StockGrader.BL;
using StockGrader.DAL.Repository;
using StockGrader.BL.Writer;

namespace StockGrader.Test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
            
        }

        [Test]
        public void IncreasedPositionTest()
        {
            //Arrange
            var filePathOld = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(TestContext.CurrentContext.TestDirectory))!, "..\\TestFiles\\ARK_ORIGINAL.csv");
            var filePathNew = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(TestContext.CurrentContext.TestDirectory))!, "..\\TestFiles\\ARK_CHANGED_INCREASED_TESLA.csv");

            DiffProvider diffProvider = GetDiffProvider(filePathOld, filePathNew);

            //Act
            var writer = new ConsoleWriter();
            writer.Write(diffProvider);

            //Assert
            Assert.That(GetRowCount(writer.IncreasedText) == 2);
            Assert.That(GetRowCount(writer.ReducedText) == 2);
            Assert.That(GetRowCount(writer.RemovedText) == 2);

        }

        private static int GetRowCount(string value)
        {
            return value.Split(Environment.NewLine).Where(x => !string.IsNullOrEmpty(x)).ToArray().Length;
        }

        [Test]
        public void ReducedPositionTest()
        {
            //Arrange
            var filePathOld = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(TestContext.CurrentContext.TestDirectory))!, "..\\TestFiles\\ARK_ORIGINAL.csv");
            var filePathNew = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(TestContext.CurrentContext.TestDirectory))!, "..\\TestFiles\\ARK_CHANGED_REDUCED_TESLA.csv");
            DiffProvider diffProvider = GetDiffProvider(filePathOld, filePathNew);

            //Act
            var writer = new ConsoleWriter();
            writer.Write(diffProvider);

            //Assert
            Assert.That(GetRowCount(writer.NewText) == 2);
            Assert.That(GetRowCount(writer.IncreasedText) == 2);
            Assert.That(GetRowCount(writer.RemovedText) == 2);
        }

        [Test]
        public void RemovedPositionTest()
        {
            //Arrange
            var filePathOld = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(TestContext.CurrentContext.TestDirectory))!, "..\\TestFiles\\ARK_ORIGINAL.csv");
            var filePathNew = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(TestContext.CurrentContext.TestDirectory))!, "..\\TestFiles\\ARK_CHANGED_REMOVED_TESLA.csv");
            DiffProvider diffProvider = GetDiffProvider(filePathOld, filePathNew);

            //Act
            var writer = new ConsoleWriter();
            writer.Write(diffProvider);

            //Assert
            Assert.That(GetRowCount(writer.NewText) == 2);
            Assert.That(GetRowCount(writer.ReducedText) == 2);
            Assert.That(GetRowCount(writer.IncreasedText) == 2);
        }

        [Test]
        public void UnchangedPositionTest()
        {
            //Arrange
            var filePathOld = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(TestContext.CurrentContext.TestDirectory))!, "..\\TestFiles\\ARK_ORIGINAL.csv");
            var filePathNew = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(TestContext.CurrentContext.TestDirectory))!, "..\\TestFiles\\ARK_UNCHANGED.csv");
            DiffProvider diffProvider = GetDiffProvider(filePathOld, filePathNew);

            //Act
            var writer = new ConsoleWriter();
            writer.Write(diffProvider);

            //Assert
            Assert.That(GetRowCount(writer.NewText) == 2);
            Assert.That(GetRowCount(writer.IncreasedText) == 2);
            Assert.That(GetRowCount(writer.ReducedText) == 2);
            Assert.That(GetRowCount(writer.RemovedText) == 2);
        }

        [Test]
        public void NewPositionTest()
        {
            //Arrange
            var filePathOld = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(TestContext.CurrentContext.TestDirectory))!, "..\\TestFiles\\ARK_ORIGINAL.csv");
            var filePathNew = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(TestContext.CurrentContext.TestDirectory))!, "..\\TestFiles\\ARK_CHANGED_ADD_CUSTOMFIRM.csv");
            DiffProvider diffProvider = GetDiffProvider(filePathOld, filePathNew);

            //Act
            var writer = new ConsoleWriter();
            writer.Write(diffProvider);

            //Assert
            Assert.That(GetRowCount(writer.IncreasedText) == 2);
            Assert.That(GetRowCount(writer.ReducedText) == 2);
            Assert.That(GetRowCount(writer.RemovedText) == 2);
        }

        private DiffProvider GetDiffProvider(string filePathOld, string filePathNew)
        {
            var stockRepositoryOld = new StockDiscRepository(Mock.Of<FileRepository>(), new Uri("http://www.uri.cz"), filePathOld);
            var stockRepositoryNew = new StockDiscRepository(Mock.Of<FileRepository>(), new Uri("http://www.uri.cz"), filePathNew);
            var stockReportOld = stockRepositoryOld.GetLast();
            var stockReportNew = stockRepositoryNew.GetLast();

            var diffProvider = new DiffProvider();
            diffProvider.CalculateDiff(stockReportOld.Entries, stockReportNew.Entries);
            return diffProvider;
        }
    }
}