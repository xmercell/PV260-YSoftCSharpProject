using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StockGrader.BL;
using StockGrader.DAL;
using StockGrader.Runner;
using StockGrader.Runner.Exception;
using System.Reflection;

// TODO: configure from out (appsettings or something like that - not hardcoded)
var filePath = Path.Combine(Assembly.GetExecutingAssembly().Location, "..\\..\\..\\..\\..\\StockGrader.Runner\\StockFiles\\ARK_ORIGINAL.csv");

var builder = new ConfigurationBuilder()
        .SetBasePath(new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName)
        .AddJsonFile("config.json", optional: false);

var config = builder.Build();

var serviceCollection = new ServiceCollection()
    .AddTransient<IConfiguration>(provider => config);
serviceCollection.InstallRunner();
serviceCollection.InstallDal(new Uri(config.GetValue<string>("StockUrl")), filePath);
serviceCollection.InstallBl();

var serviceProvider = serviceCollection.BuildServiceProvider();

var runner = serviceProvider.GetService<IRunner>();
try
{
    await runner.Run();
}
catch (NewStocksNotAvailableException ex)
{
    Console.WriteLine(ex.Message);
}