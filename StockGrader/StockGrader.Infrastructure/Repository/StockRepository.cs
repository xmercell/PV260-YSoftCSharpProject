    using StockGrader.Domain.Model;

    namespace StockGrader.Infrastructure.Repository
    {
        public class StockRepository : IStockRepository
        {
            private readonly IFileRepository _fileRepository;
            private readonly Uri _holdingsSheetUri;
            private readonly string _reportFilepath;

            public StockRepository(IFileRepository fileRepository, Uri holdingsSheetUri, string reportFilepath)
            {
                _fileRepository = fileRepository;
                _holdingsSheetUri = holdingsSheetUri;
                _reportFilepath = reportFilepath;
            }
            
            public async Task<StockReport> FetchNew()
            {
                await _fileRepository.Fetch(_holdingsSheetUri, _reportFilepath);
                var report = new StockReport(_reportFilepath);
                return report;
            }
        }
    }
