using CsvHelper;
using System.Globalization;

namespace StockGrader.Domain.Model
{
    public class StockReport
    {
        public IEnumerable<ReportEntry> Entries { get; set; } = new List<ReportEntry>();

        public StockReport(string filePath)
        {
            using var reader = new StreamReader(filePath);
            // skip header
            reader.ReadLine();

            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            Entries = csv.GetRecords<ReportEntry>();
        }
    }
}
