using StockGrader.BL;
using StockGrader.BL.Writer;
using StockGrader.DAL.Exception;
using StockGrader.DAL.Model;
using StockGrader.DAL.Repository;

namespace StockGrader.StockComparisonRunner
{
    public class Runner : IRunner
    {
        private readonly IStockRepository _stockRepository;
        private readonly IDiffProvider _diffProvider;
        private readonly IWriter _writer;

        public Runner(IStockRepository stockRepository, IDiffProvider diffProvider, IWriter writer)
        {
            _stockRepository = stockRepository;
            _diffProvider = diffProvider;
            _writer = writer;
        }

        public async Task Run()
        {
            StockReport oldReport, newReport;

            try
            {
                oldReport = _stockRepository.GetLast();
                await _stockRepository.FetchNew();
                newReport = _stockRepository.GetLast();
            }
            catch (Exception ex) 
            {
                if (ex is LastStockNotFoundException || ex is NewStocksNotAvailableException)
                {
                    _writer.WriteError($"Stock grading failed because of: {ex.Message}");
                    return;
                }

                _writer.WriteError($"Unknown error occured: {ex.Message}");
                throw;
            }

            var diff = _diffProvider.CalculateDiff(oldReport.Entries, newReport.Entries);
            _writer.WriteStockComparison(diff);
        }
    }
}
