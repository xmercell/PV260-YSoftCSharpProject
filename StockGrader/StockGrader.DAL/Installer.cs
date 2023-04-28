using Microsoft.Extensions.DependencyInjection;
using StockGrader.DAL.Repository;

namespace StockGrader.DAL
{
    public static class Installer
    {
        public static void InstallDal(this IServiceCollection collection, Uri holdingsSheetUri, 
            string userAgentHeader, string commonUserAgent, string endpointUri, string primaryKey, string databaseName, string containerName)
        {
            collection.AddTransient<IStockRepository>(provider => 
                new StockDiscRepository(holdingsSheetUri, userAgentHeader, commonUserAgent, endpointUri, primaryKey, databaseName, containerName));
        }
    }
}
