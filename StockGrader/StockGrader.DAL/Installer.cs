using Microsoft.Extensions.DependencyInjection;
using StockGrader.DAL.Repository;

namespace StockGrader.DAL
{
    public static class Installer
    {
        public static void InstallDal(this IServiceCollection collection, Uri holdingsSheetUri, string reportFilePath)
        {
            collection.AddTransient<IFileRepository, FileRepository>();
            collection.AddTransient<IStockRepository>(provider => 
                new StockDiscRepository(provider.GetService<IFileRepository>(), holdingsSheetUri, reportFilePath));
        }
    }
}
