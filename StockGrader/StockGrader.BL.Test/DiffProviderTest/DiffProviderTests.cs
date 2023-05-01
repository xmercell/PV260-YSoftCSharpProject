using FakeItEasy;
using StockGrader.BL.Exception;
using StockGrader.DAL.Model;

namespace StockGrader.BL.Test.DiffProviderTest
{
    public class DiffProviderTests
    {
        [Test]
        public void CalculateDiff_ShouldPopulateAllPropertiesCorrectly()
        {
            new When()
                .LoadDiffEntries()
                .CalculateDiff()
                .AssertDiffPositions();
        }

        [Test]
        public void ProcessEntries_EmptyInput_ReturnsEmptyDictionary()
        {
            new When()
                .EmptyEntries()
                .ProcessCurrentEntries()
                .AssertEntriesEmpty();
        }

        [Test]
        public void ProcessEntries_ValidEntries_ReturnsProcessedEntries()
        {
            new When()
                .ValidEntries()
                .ProcessCurrentEntries()
                .AssertEntriesValues();
        }

        [Test]
        public void ProcessEntries_DuplicateEntries_ThrowsDuplicateRecordException()
        {
            new When()
                .DuplicateEntries()
                .AssertDuplicateEntriesThrowEx();
        }

        [Test]
        public void ProcessEntries_MixedValidAndDuplicateEntries_ThrowsDuplicateRecordException()
        {
            new When()
                .ValidAndDuplicateEntries()
                .AssertDuplicateEntriesThrowEx();
        }

        [TestCase(100, 100, 0)]
        [TestCase(100, 200, 100)]
        [TestCase(200, 100, -50)]
        [TestCase(0, 100, 100)]
        [TestCase(100, 0, -100)]
        [TestCase(0, 0, 0)]
        public void ComputeShareChangePercentage_GivenOldAndNewShare_ReturnsPercentageChange(int oldShare, int newShare, double expectedPercentage)
        {
            new When()
                .ComputePercentages(oldShare, newShare, expectedPercentage)
                .AssertCorrectPercentage();
        }
    }
}
