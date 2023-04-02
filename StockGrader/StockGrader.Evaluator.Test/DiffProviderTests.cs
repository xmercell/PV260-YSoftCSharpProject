using StockGrader.Domain.Model;
using StockGrader.Evaluator.Exceptions;
using StockGrader.Evaluator.Model;

namespace StockGrader.Evaluator.Test
{
    public class DiffProviderTests
    {
        private DiffProvider diffprovider;

        [SetUp]
        public void Setup()
        {
            diffprovider = new DiffProvider();
        }

        [Test]
        public void ProcessEntries_EmptyInput_ReturnsEmptyDictionary()
        {
            // Arrange
            var entries = new List<ReportEntry>();

            // Act
            var result = diffprovider.ProcessEntries(entries);

            // Assert
            Assert.IsEmpty(result);
        }

        [Test]
        public void ProcessEntries_ValidEntries_ReturnsProcessedEntries()
        {
            // Arrange
            var entries = new List<ReportEntry>
            {
                new ReportEntry("1/01/2023,Fund1,Company1,Ticker1,Cusip1,100,1000,10"),
                new ReportEntry("1/01/2023,Fund1,Company2,Ticker2,Cusip2,200,2000,20")
            };

            // Act
            var result = diffprovider.ProcessEntries(entries);
            // Assert
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result["Ticker1"].CompanyName, Is.EqualTo("Company1"));
            Assert.That(result["Ticker1"].Ticker, Is.EqualTo("Ticker1"));
            Assert.That(result["Ticker2"].CompanyName, Is.EqualTo("Company2"));
            Assert.That(result["Ticker2"].Ticker, Is.EqualTo("Ticker2"));
        }

        [Test]
        public void ProcessEntries_DuplicateEntries_ThrowsDuplicateRecordException()
        {
            // Arrange
            var entries = new List<ReportEntry>
            {
                new ReportEntry("1/01/2023,Fund1,Company1,Ticker1,Cusip1,100,1000,10"),
                new ReportEntry("1/01/2023,Fund1,Company1,Ticker1,Cusip1,100,1000,10")
            };

            // Act & Assert
            Assert.Throws<DuplicateRecordException>(() => diffprovider.ProcessEntries(entries));
        }

        [Test]
        public void ProcessEntries_MixedValidAndDuplicateEntries_ThrowsDuplicateRecordException()
        {
            // Arrange
            var entries = new List<ReportEntry>
            {
                new ReportEntry("1/01/2023,Fund1,Company1,Ticker1,Cusip1,100,1000,10"),
                new ReportEntry("1/01/2023,Fund1,Company2,Ticker2,Cusip2,200,2000,20"),
                new ReportEntry("1/01/2023,Fund1,Company2,Ticker2,Cusip2,200,2000,20")
            };

            // Act & Assert
            Assert.Throws<DuplicateRecordException>(() => diffprovider.ProcessEntries(entries));
        }

        [TestCase(100, 100, 0)]
        [TestCase(100, 200, 100)]
        [TestCase(200, 100, -50)]
        [TestCase(0, 100, 100)]
        [TestCase(100, 0, -100)]
        [TestCase(0, 0, 0)]
        public void ComputeShareChangePercentage_GivenOldAndNewShare_ReturnsPercentageChange(int oldShare, int newShare, double expectedPercentage)
        {
            // Act
            double result = diffprovider.ComputeShareChangePercentage(oldShare, newShare);
            
            // Assert
            Assert.That(result, Is.EqualTo(expectedPercentage).Within(0.0001));
        }

    }
}