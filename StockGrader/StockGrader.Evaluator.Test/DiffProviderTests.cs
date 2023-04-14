using StockGrader.DAL.Model;
using StockGrader.BL.Exception;

namespace StockGrader.BL.Test
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
                new ReportEntry
                {
                    Date = new DateTime(2023, 1, 1),
                    Fund = "Fund1",
                    CompanyName = "Company1",
                    Ticker = "Ticker1",
                    Cusip = "Cusip1",
                    Shares = 100,
                    MarketValue = 1000,
                    Weight = 10
                },
                new ReportEntry
                {
                    Date = new DateTime(2023, 1, 1),
                    Fund = "Fund1",
                    CompanyName = "Company2",
                    Ticker = "Ticker2",
                    Cusip = "Cusip2",
                    Shares = 200,
                    MarketValue = 2000,
                    Weight = 20
                }
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
                new ReportEntry
                {
                    Date = new DateTime(2023, 1, 1),
                    Fund = "Fund1",
                    CompanyName = "Company1",
                    Ticker = "Ticker1",
                    Cusip = "Cusip1",
                    Shares = 100,
                    MarketValue = 1000,
                    Weight = 10
                },
                new ReportEntry
                {
                    Date = new DateTime(2023, 1, 1),
                    Fund = "Fund1",
                    CompanyName = "Company1",
                    Ticker = "Ticker1",
                    Cusip = "Cusip1",
                    Shares = 100,
                    MarketValue = 1000,
                    Weight = 10
                }
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
                new ReportEntry
                {
                    Date = new DateTime(2023, 1, 1),
                    Fund = "Fund1",
                    CompanyName = "Company1",
                    Ticker = "Ticker1",
                    Cusip = "Cusip1",
                    Shares = 100,
                    MarketValue = 1000,
                    Weight = 10
                },
                new ReportEntry
                {
                    Date = new DateTime(2023, 1, 1),
                    Fund = "Fund1",
                    CompanyName = "Company2",
                    Ticker = "Ticker2",
                    Cusip = "Cusip2",
                    Shares = 200,
                    MarketValue = 2000,
                    Weight = 20
                },
                new ReportEntry
                {
                    Date = new DateTime(2023, 1, 1),
                    Fund = "Fund1",
                    CompanyName = "Company2",
                    Ticker = "Ticker2",
                    Cusip = "Cusip2",
                    Shares = 200,
                    MarketValue = 2000,
                    Weight = 20
                }
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