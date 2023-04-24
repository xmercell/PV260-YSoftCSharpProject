using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using StockGrader.DiscordBot.Services;

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
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("Config.json");

            var configuration = configBuilder.Build();
            Console.WriteLine(configuration);

            return new ServiceCollection()
                .AddSingleton<IConfiguration>(configuration)
                .AddSingleton<DiscordSocketClient>()
                .AddSingleton<CommandService>()
                .AddSingleton<Bot>()
                .BuildServiceProvider();
        }
    }
}