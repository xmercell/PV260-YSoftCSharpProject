using StockGrader.Domain.Model;

namespace StockGrader.Infrastructure.Repository
{
    public interface IStockRepository
    {
        /// <summary>
        /// Fetch fresh stock report from csv file on the web address: 
        /// </summary>
        /// <param name="holdingsSheetUri">URI of the file to download</param>
        /// <param name="reportFilepath">filepath to be used for the new report</param>
        /// <returns>New stock report parsed from csv</returns>
        Task<StockReport> FetchNew(Uri holdingsSheetUri, string reportFilepath);

    }
}
