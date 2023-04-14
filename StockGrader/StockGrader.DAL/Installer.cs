using Microsoft.Extensions.DependencyInjection;
using StockGrader.DAL.Repository;
using System.Runtime.CompilerServices;

namespace StockGrader.DAL
{
    public static class Installer
    {
        public static void InstallDal(this IServiceCollection collection)
        {
            collection.AddTransient<IFileRepository, FileRepository>();
            collection.AddTransient<IStockRepository, StockRepository>();
        }
    }
}
