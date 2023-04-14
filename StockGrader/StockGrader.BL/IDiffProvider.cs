using StockGrader.DAL.Model;

namespace StockGrader.BL
{
    public interface IDiffProvider
    {
        public void CalculateDiff(IEnumerable<ReportEntry> oldEntries, IEnumerable<ReportEntry> newEntries);

    }
}