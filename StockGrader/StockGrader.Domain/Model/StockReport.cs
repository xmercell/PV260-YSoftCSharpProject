namespace StockGrader.Domain.Model
{
    public class StockReport
    {
        public IEnumerable<ReportEntry> Entries { get; set; } = new List<ReportEntry>();

        public void FromCSV(string path)
        {
            using (var reader = new StreamReader(@path))
            {
                reader.ReadLine(); //header

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var entry = new ReportEntry();
                    entry.FromCSV(line);
                    _ = Entries.Append(entry);
                }
            }
        }
    }
}
