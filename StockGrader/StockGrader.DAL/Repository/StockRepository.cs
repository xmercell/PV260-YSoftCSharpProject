using CsvHelper;
using CsvHelper.Configuration;
using StockGrader.DAL.Model;
using System.Globalization;

namespace StockGrader.DAL.Repository
{
    public class StockRepository : IStockRepository
    {
        private readonly IFileRepository _fileRepository;
        private readonly Uri _holdingsSheetUri;
        private readonly string _reportFilePath;

        public StockRepository(IFileRepository fileRepository, Uri holdingsSheetUri, string reportFilePath)
        {
            _fileRepository = fileRepository;
            _holdingsSheetUri = holdingsSheetUri;
            _reportFilePath = reportFilePath;
        }

        public async Task FetchNew()
        {
            await _fileRepository.Fetch(_holdingsSheetUri, _reportFilePath);
        }

        public StockReport GetLast()
        {
            using var reader = new StreamReader(_reportFilePath);

            using var csv = new CsvReader(reader, GetConfig());
            csv.Context.RegisterClassMap(new ReportEntryMap());
            
            var rows = csv.GetRecords<ReportEntry>().ToList();
            return new StockReport { Entries = rows };
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
