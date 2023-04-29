using StockGrader.DAL.Model;

namespace StockGrader.DAL.Repository
{
    public interface IStockRepository
    {
        /// <summary>
        /// Fetch fresh stock report from csv file on the web address: 
        /// </summary>
        /// <returns>New stock report parsed from csv</returns>
        Task FetchNew();

        StockReport GetLast();

        StockReport GetByDate(DateTime date);
    }
}
