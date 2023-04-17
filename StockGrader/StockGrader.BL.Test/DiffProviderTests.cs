using FakeItEasy;
using StockGrader.BL.Exception;
using StockGrader.DAL.Model;

namespace StockGrader.BL.Test
{
    public class DiffProviderTests
    {
        private DiffProvider _diffProvider;

        [SetUp]
        public void SetUp()
        {
            _diffProvider = new DiffProvider();
            
        }

        [Test]
        public void CalculateDiff_ShouldPopulateAllPropertiesCorrectly()
        {
            var oldEntries = new List<ReportEntry>
            {
                new ReportEntry
                {
                    CompanyName = "Company A",
                    Ticker = "A",
                    Shares = 1000,
                    Weight = 1
                },
                new ReportEntry
                {
                    CompanyName = "Company B",
                    Ticker = "B",
                    Shares = 2000,
                    Weight = 2
                }
            };

            // Test data for new entries
            var newEntries = new List<ReportEntry>
            {
                new ReportEntry
                {
                    CompanyName = "Company A",
                    Ticker = "A",
                    Shares = 1200,
                    Weight = 12
                },
                new ReportEntry
                {
                    CompanyName = "Company C",
                    Ticker = "C",
                    Shares = 1500,
                    Weight = 15
                }
            };
            // Arrange
            var diffProvider = A.Fake<IDiffProvider>();
            A.CallTo(() => diffProvider.CalculateDiff(A<IEnumerable<ReportEntry>>.Ignored, A<IEnumerable<ReportEntry>>.Ignored)).Invokes(() => _diffProvider.CalculateDiff(oldEntries, newEntries));

            // Act
            diffProvider.CalculateDiff(oldEntries, newEntries);

            // Assert
            Assert.That(_diffProvider.NewPositions.Count(), Is.EqualTo(1));
            Assert.That(_diffProvider.UnchangedPositions.Count(), Is.EqualTo(0));
            Assert.That(_diffProvider.IncreasedPositions.Count(), Is.EqualTo(1));
            Assert.That(_diffProvider.ReducedPositions.Count(), Is.EqualTo(0));
            Assert.That(_diffProvider.RemovedPositions.Count(), Is.EqualTo(1));
        }

        [Test]
        public void ProcessEntries_EmptyInput_ReturnsEmptyDictionary()
        {
            // Arrange
            var entries = new List<ReportEntry>();

            // Act
            var result = _diffProvider.ProcessEntries(entries);

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
            var result = _diffProvider.ProcessEntries(entries);
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
            Assert.Throws<DuplicateRecordException>(() => _diffProvider.ProcessEntries(entries));
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
            Assert.Throws<DuplicateRecordException>(() => _diffProvider.ProcessEntries(entries));
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
            double result = _diffProvider.ComputeShareChangePercentage(oldShare, newShare);

            // Assert
            Assert.That(result, Is.EqualTo(expectedPercentage).Within(0.0001));
        }

    }
}
