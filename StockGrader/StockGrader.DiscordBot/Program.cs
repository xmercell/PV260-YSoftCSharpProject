using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using StockGrader.DiscordBot.Services;
using StockGrader.StockComparisonRunner;
using StockGrader.BL;
using StockGrader.DAL;
using StockGrader.DiscordBot.Configurations;

namespace StockGrader.DiscordBot
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var services = ConfigureServices();
            var bot = services.GetRequiredService<Bot>();
            await bot.StartAsync();
            await Task.Delay(-1); // Prevent the program from exiting immediately
        }

        private static ServiceProvider ConfigureServices()
        {   
            // The configuration for http client
            var stockUrl = new Uri(System.Configuration.ConfigurationManager.AppSettings["StockUrl"]);        
            var userAgentHeader = System.Configuration.ConfigurationManager.AppSettings["UserAgentHeader"];
            var commonUserAgent = System.Configuration.ConfigurationManager.AppSettings["CommonUserAgent"];

            // The configuration for connecting to Azure database
            var endpointUri = System.Configuration.ConfigurationManager.AppSettings["EndPointUri"];
            var primaryKey = System.Configuration.ConfigurationManager.AppSettings["PrimaryKey"];
            var databaseName = System.Configuration.ConfigurationManager.AppSettings["DatabaseName"];
            var containerName = System.Configuration.ConfigurationManager.AppSettings["ContainerName"];

            // The confiuration for bot
            var token = System.Configuration.ConfigurationManager.AppSettings["token"];
            var prefix = System.Configuration.ConfigurationManager.AppSettings["prefix"];

            var serviceCollection = new ServiceCollection();
            serviceCollection.InstallStockComparisonRunner();
            serviceCollection.InstallDal(stockUrl, userAgentHeader, commonUserAgent, endpointUri, primaryKey, databaseName, containerName);
            serviceCollection.InstallBl();

            serviceCollection
                .AddSingleton<DiscordSocketClient>()
                .AddSingleton<CommandService>()
                .AddSingleton<BotConfig>(provider => new BotConfig(token, prefix))
                .AddSingleton<Bot>();

            return serviceCollection.BuildServiceProvider();
    }
}