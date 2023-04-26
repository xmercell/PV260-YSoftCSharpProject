using StockGrader.DAL.Model;
using StockGrader.BL.Exception;
using StockGrader.BL.Model;

namespace StockGrader.BL
{
    public class DiffProvider : IDiffProvider
    {
        public Diff CalculateDiff(IEnumerable<ReportEntry> oldEntries, IEnumerable<ReportEntry> newEntries)
        {
            var oldProcessed = ProcessEntries(oldEntries);
            var newProcessed = ProcessEntries(newEntries);
            return ParseProcessedEntries(oldProcessed, newProcessed);
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

        public Diff ParseProcessedEntries(IDictionary<string, ProcessedEntry> oldEntries, IDictionary<string, ProcessedEntry> newEntries)
        {
            var diff = new Diff();
            foreach (var newEntry in newEntries)
            {
                if (!oldEntries.ContainsKey(newEntry.Key))
                {
                    var newPosition = new Position(newEntry.Value.CompanyName,
                                                    newEntry.Value.Ticker,
                                                    newEntry.Value.Shares,
                                                    newEntry.Value.Weight);

                    diff.NewPositions.Add(newPosition);
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
                    diff.UnchangedPositions.Add(unchangedPosition);
                }
                else if (shareChange < 0)
                { 
                    var reducedPosition = new UpdatedPosition(newEntry.Value.CompanyName,
                                                        newEntry.Value.Ticker,
                                                        newEntry.Value.Shares,
                                                        newEntry.Value.Weight,
                                                         shareChange);
                    diff.ReducedPositions.Add(reducedPosition);
                }
                else
                {
                    var increasedPosition = new UpdatedPosition(newEntry.Value.CompanyName,
                                                        newEntry.Value.Ticker,
                                                        newEntry.Value.Shares,
                                                        newEntry.Value.Weight,
                                                         shareChange);
                    diff.IncreasedPositions.Add(increasedPosition);
                }
                oldEntries.Remove(newEntry.Key);
            }
            foreach(var oldEntry in oldEntries)
            {
                var removedPosition = new RemovedPosition(oldEntry.Value.CompanyName,
                                                    oldEntry.Value.Ticker);
                diff.RemovedPositions.Add(removedPosition);
            }

            return diff;
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
