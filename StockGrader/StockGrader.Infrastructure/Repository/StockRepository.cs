using CsvHelper;
using CsvHelper.Configuration;
using StockGrader.Domain.Model;
using StockGrader.Infrastructure.Services;
using System.Globalization;

namespace StockGrader.Infrastructure.Repository
{
    public class StockRepository : IStockRepository
    {
        private readonly IFileService _fileService;
        private readonly Uri _holdingsSheetUri;
        private readonly string _reportFilePath;

        public StockRepository(IFileService fileService, Uri holdingsSheetUri, string reportFilePath)
        {
            _fileService = fileService;
            _holdingsSheetUri = holdingsSheetUri;
            _reportFilePath = reportFilePath;
        }

        public async Task FetchNew()
        {
            await _fileService.Fetch(_holdingsSheetUri, _reportFilePath);
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
