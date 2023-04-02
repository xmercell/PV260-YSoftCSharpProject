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

            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();

                if (!reader.EndOfStream)
                {
                    var reportEntry = new ReportEntry(line);
                    ((List<ReportEntry>)Entries).Add(reportEntry);
                }
            }
        }
    }
}
