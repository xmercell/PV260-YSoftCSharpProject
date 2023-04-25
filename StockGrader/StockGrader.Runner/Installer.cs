using Microsoft.Extensions.DependencyInjection;

namespace StockGrader.Runner
{
    public static class Installer
    {
        public static void InstallRunner(this IServiceCollection collection)
        {
            collection.AddTransient<IRunner, Runner>();
        }
    }
}
