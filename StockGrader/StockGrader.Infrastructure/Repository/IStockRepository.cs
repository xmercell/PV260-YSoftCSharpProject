using StockGrader.Domain.Model;

namespace StockGrader.Infrastructure.Repository
{
    public interface IStockRepository
    {
        /// <summary>
        /// Fetch fresh new stock report
        /// </summary>
        /// <returns></returns>
        Task FetchNew();

        /// <summary>
        /// Get last fetched stock report
        /// </summary>
        /// <returns>Last stock report</returns>
        StockReport GetLast();
    }
}
