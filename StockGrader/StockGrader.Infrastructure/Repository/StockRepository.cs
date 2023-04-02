    using StockGrader.Domain.Model;

    namespace StockGrader.Infrastructure.Repository
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
                return new StockReport(_reportFilePath);
            }
        }
    }
