using Microsoft.Extensions.DependencyInjection;

namespace StockGrader.StockComparisonRunner
{
    public static class Installer
    {
        public static void InstallStockComparisonRunner(this IServiceCollection collection)
        {
            collection.AddTransient<IRunner, Runner>();
        }
    }
}
