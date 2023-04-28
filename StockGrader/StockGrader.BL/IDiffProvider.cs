using StockGrader.BL.Model;
using StockGrader.DAL.Model;

namespace StockGrader.BL
{
    public interface IDiffProvider
    {
        public Diff CalculateDiff(IEnumerable<ReportEntry> oldEntries, IEnumerable<ReportEntry> newEntries);

    }
}