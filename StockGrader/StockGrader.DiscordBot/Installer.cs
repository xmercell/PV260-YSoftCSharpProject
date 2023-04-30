using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using StockGrader.DiscordBot.Configurations;

namespace StockGrader.DiscordBot;

public static class Installer
{
    public static void InstallBot(this ServiceCollection serviceCollection, string? token, string? prefix)
    {
        serviceCollection.AddSingleton<DiscordSocketClient>()
            .AddSingleton<CommandService>()
            .AddSingleton<BotConfig>(provider => new BotConfig(token, prefix))
            .AddSingleton<Bot>();
    }
}