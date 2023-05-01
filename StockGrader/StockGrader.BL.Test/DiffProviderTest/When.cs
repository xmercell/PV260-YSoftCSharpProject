using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FakeItEasy;
using Microsoft.Azure.Cosmos.Serialization.HybridRow;
using StockGrader.BL.Exception;
using StockGrader.BL.Model;
using StockGrader.DAL.Model;

namespace StockGrader.BL.Test.DiffProviderTest
{
    public class When
    {
        private DiffProvider _diffProvider;
        private Tuple<List<ReportEntry>, List<ReportEntry>> currentDiffEntries;
        private Diff currentDiff;
        private List<ReportEntry> currentEntries;
        double currentSharePercentage;
        double expectedSharePercentage;
        private IDictionary<string, ProcessedEntry> processedEntries;

        public When()
        {
            _diffProvider = new DiffProvider();
        }

        public void AssertDuplicateEntriesThrowEx()
        {
            Assert.Throws<DuplicateRecordException>(() => _diffProvider.ProcessEntries(currentEntries));
        }

        public When DuplicateEntries()
        {
            currentEntries = new List<ReportEntry>
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
            return this;
        }

        public When ValidAndDuplicateEntries()
        {
            currentEntries = new List<ReportEntry>
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
            return this;
        }

        public When ValidEntries()
        {
            currentEntries = new List<ReportEntry>
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
            return this;
        }

        public When EmptyEntries()
        {
           currentEntries = new List<ReportEntry>();
           return this;
        }

        public When ProcessCurrentEntries()
        {
            processedEntries = _diffProvider.ProcessEntries(currentEntries);
            return this;
        }

        public When LoadDiffEntries()
        {
            currentDiffEntries = new Tuple<List<ReportEntry>, List<ReportEntry>>(
            new List<ReportEntry>
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
            },
            new List<ReportEntry>
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
            });
            return this;
        }

        public When CalculateDiff()
        {
            var diffProvider = A.Fake<IDiffProvider>();
            A.CallTo(() => diffProvider.CalculateDiff(A<IEnumerable<ReportEntry>>.Ignored, A<IEnumerable<ReportEntry>>.Ignored)).Returns(_diffProvider.CalculateDiff(currentDiffEntries.Item1, currentDiffEntries.Item2));

            currentDiff = diffProvider.CalculateDiff(currentDiffEntries.Item1, currentDiffEntries.Item2);
            return this;
        }

        public When ComputePercentages(int oldShare, int newShare, double expectedPercentage)
        {
            currentSharePercentage = _diffProvider.ComputeShareChangePercentage(oldShare, newShare);
            expectedSharePercentage = expectedPercentage;
            return this;
        }

        public void AssertCorrectPercentage()
        {
            Assert.That(currentSharePercentage, Is.EqualTo(expectedSharePercentage).Within(0.0001));
        }

        public void AssertDiffPositions()
        {
            Assert.That(currentDiff.NewPositions.Count(), Is.EqualTo(1));
            Assert.That(currentDiff.UnchangedPositions.Count(), Is.EqualTo(0));
            Assert.That(currentDiff.IncreasedPositions.Count(), Is.EqualTo(1));
            Assert.That(currentDiff.ReducedPositions.Count(), Is.EqualTo(0));
            Assert.That(currentDiff.RemovedPositions.Count(), Is.EqualTo(1));
        }

        public void AssertEntriesEmpty()
        {
            Assert.IsEmpty(processedEntries);
        }

        public void AssertEntriesValues()
        {
            Assert.That(processedEntries.Count, Is.EqualTo(2));
            Assert.That(processedEntries["Ticker1"].CompanyName, Is.EqualTo("Company1"));
            Assert.That(processedEntries["Ticker1"].Ticker, Is.EqualTo("Ticker1"));
            Assert.That(processedEntries["Ticker2"].CompanyName, Is.EqualTo("Company2"));
            Assert.That(processedEntries["Ticker2"].Ticker, Is.EqualTo("Ticker2"));
        }
    }
}
