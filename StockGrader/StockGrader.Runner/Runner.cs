using StockGrader.BL.Writer;
using StockGrader.BL;
using StockGrader.DAL.Repository;
using Microsoft.Extensions.Configuration;
using StockGrader.Runner.Exception;

namespace StockGrader.Runner
{
    public class Runner : IRunner
    {
        private readonly IStockRepository _stockRepository;
        private readonly IDiffProvider _diffProvider;
        private readonly IWriter _writer;
        private readonly IConfiguration _configuration;

        public Runner(IStockRepository stockRepository, IDiffProvider diffProvider, IWriter writer, IConfiguration configuration)
        {
            _stockRepository = stockRepository;
            _diffProvider = diffProvider;
            _writer = writer;
            _configuration = configuration;
        }

        public async Task Run()
        {
            var oldReport = _stockRepository.GetLast();

            try
            {
                await _stockRepository.FetchNew();
            }
            catch (HttpRequestException)
            {
                throw new NewStocksNotAvailableException(new Uri(_configuration.GetSection("StockUrl").Value));
            }

            var newReport = _stockRepository.GetLast();

            var diff = _diffProvider.CalculateDiff(oldReport.Entries, newReport.Entries);
            _writer.Write(diff);
        }
    }
}
