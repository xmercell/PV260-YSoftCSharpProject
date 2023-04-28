using Microsoft.Extensions.DependencyInjection;
using StockGrader.DAL.Repository;

namespace StockGrader.DAL
{
    public static class Installer
    {
        public static void InstallDal(this IServiceCollection collection, Uri holdingsSheetUri, string reportFilePath, 
            string userAgentHeader, string commonUserAgent)
        {
            collection.AddTransient<IStockRepository>(provider => 
                new StockDiscRepository(holdingsSheetUri, reportFilePath, userAgentHeader, commonUserAgent));
        }
    }
}
