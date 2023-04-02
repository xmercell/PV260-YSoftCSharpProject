using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

namespace StockGrader.Domain.Model
{
    public class StockReport
    {
        public IEnumerable<ReportEntry> Entries { get; set; } = new List<ReportEntry>();

        public StockReport(string filePath)
        {
            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, GetConfig());
            csv.Context.RegisterClassMap(new ReportEntryMap());
            Entries = csv.GetRecords<ReportEntry>().ToList();
        }

        private CsvConfiguration GetConfig()
        {
            return new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                MissingFieldFound = null,
                ShouldSkipRecord = (row) =>
                {
                    if (row.Row.HeaderRecord != null)
                    {
                        return !DateTime.TryParse(row.Row.GetField("date"), out _);
                    }
                    return false;
                }
            };
        }
    }
}
