namespace StockGrader.Domain.Model
{
    public class StockReport
    {
        public IEnumerable<ReportEntry> Entries { get; set; } = new List<ReportEntry>();

        public static StockReport FromCSV(string path)
        {
            StockReport report = new StockReport
            {
                Entries = new List<ReportEntry>()
            };

            using (var reader = new StreamReader(@path))
            {
                reader.ReadLine(); //header

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    report.Entries.Append(ReportEntry.FromCSV(line));
                }
            }

            return report;
        }
    }
}
