using StockGrader.BL.Model;
using StockGrader.DAL.Model;

namespace StockGrader.BL
{
    public interface IDiffProvider
    {
        IEnumerable<Position> NewPositions { get; }
        IEnumerable<Position> UnchangedPositions { get; }
        IEnumerable<UpdatedPosition> IncreasedPositions { get; }
        IEnumerable < UpdatedPosition > ReducedPositions { get; }
        IEnumerable<RemovedPosition> RemovedPositions { get; }

        public void CalculateDiff(IEnumerable<ReportEntry> oldEntries, IEnumerable<ReportEntry> newEntries);

    }
}