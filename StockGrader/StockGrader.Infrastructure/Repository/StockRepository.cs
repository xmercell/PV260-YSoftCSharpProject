    using StockGrader.Domain.Model;

    namespace StockGrader.Infrastructure.Repository
    {
        public class StockRepository : IStockRepository
        {
            private readonly IFileRepository _fileRepository;

            public StockRepository(IFileRepository fileRepository)
            {
                _fileRepository = fileRepository;
            }
            
            public async Task<StockReport> FetchNew(Uri holdingsSheetUri, string reportFilepath)
            {
                await _fileRepository.Fetch(holdingsSheetUri, reportFilepath);
                var report = new StockReport(reportFilepath);
                return report;
            }
        }
    }
