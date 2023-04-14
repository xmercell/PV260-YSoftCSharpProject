using StockGrader.DAL.Model;

namespace StockGrader.Evaluator
{
    public interface IDiffProvider
    {
        public void CalculateDiff(IEnumerable<ReportEntry> oldEntries, IEnumerable<ReportEntry> newEntries);

    }
}