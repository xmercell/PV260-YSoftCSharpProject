using StockGrader.Domain.Model;
using StockGrader.Evaluator.Model;

namespace StockGrader.Evaluator
{
    public class DiffProvider : IDiffProvider
    {
        public IEnumerable<Position> NewPositions = new List<Position>();
        public IEnumerable<Position> UnchanchedPositions = new List<Position>();
        public IEnumerable<UpdatedPosition> IncreasedPositions = new List<UpdatedPosition>();
        public IEnumerable<UpdatedPosition> ReducedPositions = new List<UpdatedPosition>();
        public IEnumerable<RemovedPosition> RemovedPositions = new List<RemovedPosition>();

        public void CalculateDiff(IEnumerable<ReportEntry> oldEntries, IEnumerable<ReportEntry> newEntries)
        {
            throw new NotImplementedException();
        }

        private IDictionary<string, ProcessedEntry> ProcessEntries(IEnumerable<ReportEntry> entries)
        {
            throw new NotImplementedException();
        }
    }
}
