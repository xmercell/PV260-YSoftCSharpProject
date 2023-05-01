
namespace StockGrader.DAL.Test
{
    public class StockDiscRepositoryTests
    {
        [Test]
        public void StockDiskRepositoryLoadsCsv()
        {
            new When()
                .LoadNewReportAsync().Result
                .AssertReportNotEmpty();
        }

        [Test]
        public void GetByRealDateTest()
        {
            new When()
                .LoadReportValidDate()
                .AssertReportEntriesCount();
        }

        [Test]
        public void GetByNonExistDateTest()
        {
            new When()
                .LoadReportInvalidDate()
                .AssertReportEmpty();
        }
    }
}
