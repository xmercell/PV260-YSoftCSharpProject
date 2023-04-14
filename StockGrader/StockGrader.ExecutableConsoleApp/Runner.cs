using StockGrader.BL.Writer;
using StockGrader.BL;
using StockGrader.DAL.Repository;

namespace StockGrader.ExecutableConsoleApp
{
    internal class Runner : IRunner
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
            var oldReport = _stockRepository.GetLast();
            await _stockRepository.FetchNew();
            var newReport = _stockRepository.GetLast();

            _diffProvider.CalculateDiff(oldReport.Entries, newReport.Entries);

            _writer.Write(_diffProvider);

        }
    }
}
