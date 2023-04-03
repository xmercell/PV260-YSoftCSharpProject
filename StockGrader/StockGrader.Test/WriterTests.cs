using Moq;
using StockGrader.Evaluator;
using StockGrader.Infrastructure.Repository;
using StockGrader.Writer;
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
            var writer = new ConsoleWriter(diffProvider);

            //Assert
            Assert.That(string.IsNullOrEmpty(writer.NewText));
            Assert.That(string.IsNullOrEmpty(writer.ReducedText));
            Assert.That(string.IsNullOrEmpty(writer.RemovedText));
            //Assert.Equals(writer.IncreasedText, "\"TESLA INC\", TSLA, spravne cisla");

        }

        [Test]
        public void ReducedPositionTest()
        {
            //Arrange
            var filePathOld = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(TestContext.CurrentContext.TestDirectory))!, "..\\TestFiles\\ARK_ORIGINAL.csv");
            var filePathNew = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(TestContext.CurrentContext.TestDirectory))!, "..\\TestFiles\\ARK_CHANGED_REDUCED_TESLA.csv");
            DiffProvider diffProvider = GetDiffProvider(filePathOld, filePathNew);

            //Act
            var writer = new ConsoleWriter(diffProvider);

            //Assert
            Assert.That(string.IsNullOrEmpty(writer.NewText));
            Assert.That(string.IsNullOrEmpty(writer.IncreasedText));
            Assert.That(string.IsNullOrEmpty(writer.RemovedText));
            //Assert.Equals(writer.ReducedText, "\"TESLA INC\", TSLA, spravne cisla");
        }

        [Test]
        public void RemovedPositionTest()
        {
            //Arrange
            var filePathOld = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(TestContext.CurrentContext.TestDirectory))!, "..\\TestFiles\\ARK_ORIGINAL.csv");
            var filePathNew = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(TestContext.CurrentContext.TestDirectory))!, "..\\TestFiles\\ARK_CHANGED_REMOVED_TESLA.csv");
            DiffProvider diffProvider = GetDiffProvider(filePathOld, filePathNew);

            //Act
            var writer = new ConsoleWriter(diffProvider);

            //Assert
            Assert.That(string.IsNullOrEmpty(writer.NewText));
            Assert.That(string.IsNullOrEmpty(writer.IncreasedText));
            Assert.That(string.IsNullOrEmpty(writer.ReducedText));
            //Assert.Equals(writer.RemovedText, "");
        }

        [Test]
        public void UnchangedPositionTest()
        {
            //Arrange
            var filePathOld = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(TestContext.CurrentContext.TestDirectory))!, "..\\TestFiles\\ARK_ORIGINAL.csv");
            var filePathNew = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(TestContext.CurrentContext.TestDirectory))!, "..\\TestFiles\\ARK_UNCHANGED.csv");
            DiffProvider diffProvider = GetDiffProvider(filePathOld, filePathNew);

            //Act
            var writer = new ConsoleWriter(diffProvider);

            //Assert
            Assert.That(string.IsNullOrEmpty(writer.NewText));
            Assert.That(string.IsNullOrEmpty(writer.IncreasedText));
            Assert.That(string.IsNullOrEmpty(writer.ReducedText));
            Assert.That(string.IsNullOrEmpty(writer.RemovedText));
            //Assert.Equals(writer.UnchangedText, "");
        }

        [Test]
        public void NewPositionTest()
        {
            //Arrange
            var filePathOld = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(TestContext.CurrentContext.TestDirectory))!, "..\\TestFiles\\ARK_ORIGINAL.csv");
            var filePathNew = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(TestContext.CurrentContext.TestDirectory))!, "..\\TestFiles\\ARK_CHANGED_ADD_CUSTOMFIRM.csv");
            DiffProvider diffProvider = GetDiffProvider(filePathOld, filePathNew);

            //Act
            var writer = new ConsoleWriter(diffProvider);

            //Assert
            Assert.That(string.IsNullOrEmpty(writer.IncreasedText));
            Assert.That(string.IsNullOrEmpty(writer.ReducedText));
            Assert.That(string.IsNullOrEmpty(writer.RemovedText));
            Assert.Equals(writer.NewText, "");
        }

        private DiffProvider GetDiffProvider(string filePathOld, string filePathNew)
        {
            var stockRepositoryOld = new StockRepository(Mock.Of<FileRepository>(), Mock.Of<Uri>(), filePathOld);
            var stockRepositoryNew = new StockRepository(Mock.Of<FileRepository>(), Mock.Of<Uri>(), filePathNew);
            var stockReportOld = stockRepositoryOld.GetLast();
            var stockReportNew = stockRepositoryNew.GetLast();

            var diffProvider = new DiffProvider();
            diffProvider.CalculateDiff(stockReportOld.Entries, stockReportNew.Entries);
            return diffProvider;
        }
    }
}