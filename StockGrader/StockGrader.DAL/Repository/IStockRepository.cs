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

        /// <summary>
        /// Returns todays report. If it is not in db, fetch it from the website: 
        /// </summary>
        /// <returns>New stock report parsed from csv</returns>
        StockReport GetCurrent();
        StockReport GetByDate(DateTime date);
    }
}
