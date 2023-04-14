using StockGrader.DAL.Model;
using StockGrader.BL.Exception;
using StockGrader.BL.Model;

namespace StockGrader.BL
{
    public class DiffProvider : IDiffProvider
    {
        public IEnumerable<Position> NewPositions = new List<Position>();
        public IEnumerable<Position> UnchangedPositions = new List<Position>();
        public IEnumerable<UpdatedPosition> IncreasedPositions = new List<UpdatedPosition>();
        public IEnumerable<UpdatedPosition> ReducedPositions = new List<UpdatedPosition>();
        public IEnumerable<RemovedPosition> RemovedPositions = new List<RemovedPosition>();

        public void CalculateDiff(IEnumerable<ReportEntry> oldEntries, IEnumerable<ReportEntry> newEntries)
        {
            var oldProcessed = ProcessEntries(oldEntries);
            var newProcessed = ProcessEntries(newEntries);
            ParseProcessedEntries(oldProcessed, newProcessed);
        }

        public IDictionary<string, ProcessedEntry> ProcessEntries(IEnumerable<ReportEntry> entries)
        {
            var processedEntries = new Dictionary<string, ProcessedEntry>();

            foreach (var reportEntry in entries)
            {
                if (processedEntries.ContainsKey(reportEntry.Ticker))
                {
                    throw new DuplicateRecordException();
                }

                processedEntries.Add(
                    reportEntry.Ticker, 
                    new ProcessedEntry(
                        reportEntry.CompanyName,
                        reportEntry.Ticker,
                        reportEntry.Shares,
                        reportEntry.Weight
                        )
                    );
            }
            return processedEntries;
        }

        public void ParseProcessedEntries(IDictionary<string, ProcessedEntry> oldEntries, IDictionary<string, ProcessedEntry> newEntries)
        {
            foreach (var newEntry in newEntries)
            {
                if (!oldEntries.ContainsKey(newEntry.Key))
                {
                    var newPosition = new Position(newEntry.Value.CompanyName,
                                                    newEntry.Value.Ticker,
                                                    newEntry.Value.Shares,
                                                    newEntry.Value.Weight);

                    ((List<Position>)NewPositions).Add(newPosition);
                    oldEntries.Remove(newEntry.Key);
                    continue;
                }
                var oldEntryValue = oldEntries[newEntry.Key];
                var shareChange = ComputeShareChangePercentage(oldEntryValue.Shares, newEntry.Value.Shares);
                if (shareChange == 0) { 
                    var unchangedPosition = new Position(newEntry.Value.CompanyName,
                                                        newEntry.Value.Ticker,
                                                        newEntry.Value.Shares,
                                                        newEntry.Value.Weight);
                    ((List<Position>)UnchangedPositions).Add(unchangedPosition);
                }
                else if (shareChange < 0)
                { 
                    var reducedPosition = new UpdatedPosition(newEntry.Value.CompanyName,
                                                        newEntry.Value.Ticker,
                                                        newEntry.Value.Shares,
                                                        newEntry.Value.Weight,
                                                         shareChange);
                    ((List<UpdatedPosition>)ReducedPositions).Add(reducedPosition);
                }
                else
                {
                    var increasedPosition = new UpdatedPosition(newEntry.Value.CompanyName,
                                                        newEntry.Value.Ticker,
                                                        newEntry.Value.Shares,
                                                        newEntry.Value.Weight,
                                                         shareChange);
                    ((List<UpdatedPosition>)IncreasedPositions).Add(increasedPosition);
                }
                oldEntries.Remove(newEntry.Key);
            }
            foreach(var oldEntry in oldEntries)
            {
                var removedPosition = new RemovedPosition(oldEntry.Value.CompanyName,
                                                    oldEntry.Value.Ticker);
                ((List<RemovedPosition>)RemovedPositions).Add(removedPosition);
            }
        }

        public double ComputeShareChangePercentage(int oldShare, int newShare)
        {
            if( oldShare == 0 )
            {
                return newShare > 0 ? 100 : 0;
            }
            var result = (((double)newShare / oldShare) - 1) * 100;
            return result;
        }
    }
}
