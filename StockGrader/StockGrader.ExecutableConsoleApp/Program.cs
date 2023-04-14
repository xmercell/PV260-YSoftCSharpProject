using Microsoft.Extensions.DependencyInjection;
using StockGrader.BL;
using StockGrader.DAL;
using StockGrader.ExecutableConsoleApp;
using System.Reflection;

internal class Program
{
    private static async Task Main(string[] args)
    {
        // TODO: configure from out (appsettings or something like that - not hardcoded)
        var filePath = Path.Combine(Assembly.GetExecutingAssembly().Location, "..\\..\\..\\..\\..\\StockGrader.Test\\TestFiles\\ARK_ORIGINAL.csv");
        var url = new Uri("https://ark-funds.com/wp-content/uploads/funds-etf-csv/ARK_INNOVATION_ETF_ARKK_HOLDINGS.csv");

        var serviceCollection = new ServiceCollection()
            .AddTransient<IRunner, Runner>();
        serviceCollection.InstallDal(url, filePath);
        serviceCollection.InstallBl();

        var serviceProvider = serviceCollection.BuildServiceProvider();

        var runner = serviceProvider.GetService<IRunner>();
        await runner.Run();
    }
}