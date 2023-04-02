using StockGrader.Domain.Model;

namespace StockGrader.Domain.Test
{
    public class ParserTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void NumbersParsingTest()
        {
            var filePath = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(TestContext.CurrentContext.TestDirectory))!, "..\\TestFiles\\ARK_CHANGED_INCREASED_TESLA.csv");
            StockReport report = new(filePath);

            Assert.That(report.Entries.First().Shares == 3896220);
            Assert.That(report.Entries.First().MarketValue == 755399133.60);
            Assert.That(report.Entries.First().Weight == 15);
        }
    }
}