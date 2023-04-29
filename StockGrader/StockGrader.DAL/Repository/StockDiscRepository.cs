using CsvHelper;
using CsvHelper.Configuration;
using StockGrader.DAL.Exception;
using StockGrader.DAL.Model;
using System.Globalization;

namespace StockGrader.DAL.Repository
{
    public class StockDiscRepository : IStockRepository
    {
        private readonly Uri _holdingsSheetUri;
        private readonly string _reportFilePath;
        private readonly string _userAgentHeader;
        private readonly string _commonUserAgent;

        public StockDiscRepository(Uri holdingsSheetUri, string reportFilePath, string userAgentHeader, string commonUserAgent)
        {
            _holdingsSheetUri = holdingsSheetUri;
            _reportFilePath = reportFilePath;
            _userAgentHeader = userAgentHeader;
            _commonUserAgent = commonUserAgent;
        }

        public async Task FetchNew()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.TryAddWithoutValidation(_userAgentHeader, _commonUserAgent);
            try
            {
                using var stream = await client.GetStreamAsync(_holdingsSheetUri);
            
                using StreamReader reader = new(stream);
                var content = reader.ReadToEnd();
                await File.WriteAllTextAsync(_reportFilePath, content);
            }
            catch (HttpRequestException)
            {
                throw new NewStocksNotAvailableException(_holdingsSheetUri);
            }
            catch (IOException ex)
            {
                throw new NewStocksNotAvailableException(ex);
            }
        }

        public StockReport GetByDate(DateTime date)
        {
            throw new NotImplementedException();
        }

        public StockReport GetLast()
        {
            try
            {
                using var reader = new StreamReader(_reportFilePath);

                using var csv = new CsvReader(reader, GetConfig());
                csv.Context.RegisterClassMap(new ReportEntryMap());

                var rows = csv.GetRecords<ReportEntry>().ToList();
                return new StockReport { Entries = rows };
            } 
            catch(FileNotFoundException)
            {
                throw new LastStockNotFoundException(_reportFilePath);
            }
            catch (IOException ex)
            {
                throw new LastStockNotFoundException(ex);
            }
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
                        return !DateTime.TryParseExact(row.Row.GetField("date"), 
                                "MM/dd/yyyy",
                                CultureInfo.InvariantCulture,
                                DateTimeStyles.None,
                                out _);
                    }
                    return false;
                }
            };
        }
    }
}
