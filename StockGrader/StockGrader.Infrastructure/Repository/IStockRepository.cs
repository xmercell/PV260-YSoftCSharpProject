using StockGrader.Domain.Model;

namespace StockGrader.Infrastructure.Repository
{
    public interface IStockRepository
    {
        /// <summary>
        /// Fetch fresh stock report from csv file on the web address: 
        /// </summary>
        /// <returns>New stock report parsed from csv</returns>
        Task<StockReport> FetchNew();

    }
}
