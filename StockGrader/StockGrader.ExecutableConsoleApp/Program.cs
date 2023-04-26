using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StockGrader.BL;
using StockGrader.DAL;
using StockGrader.StockComparisonRunner;
using System.Reflection;
using System.Security;

// build configuration
var builder = new ConfigurationBuilder()
        .SetBasePath(new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName)
        .AddJsonFile("config.json", optional: false);

var config = builder.Build();

// parse configuration
var filePath = Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.FullName, "StockFiles", config.GetValue<string>("FileName"));
var stockUrl = new Uri(config.GetValue<string>("StockUrl"));
var userAgentHeader = config.GetSection("HttpClientConfig").GetValue<string>("UserAgentHeader");
var commonUserAgent = config.GetSection("HttpClientConfig").GetValue<string>("CommonUserAgent");

var serviceCollection = new ServiceCollection();
serviceCollection.InstallStockComparisonRunner();
serviceCollection.InstallDal(stockUrl, userAgentHeader, commonUserAgent);
serviceCollection.InstallBl();

var serviceProvider = serviceCollection.BuildServiceProvider();

var runner = serviceProvider.GetService<IRunner>();
await runner.Run();