using Discord;
using Discord.WebSocket;

namespace StockGrader.DiscordBot.Extensions;

public static class SocketGuildExtension
{
    
    public static SocketTextChannel GetTextChannelByName(this SocketGuild guild, string channelName)
    {
        return guild.TextChannels.FirstOrDefault(c => c.Name.Equals(channelName, StringComparison.OrdinalIgnoreCase));
    }
    
    public static async Task<ITextChannel> EnsureChannelExistsAsync(this SocketGuild guild, string channelName)
    {
        var existingChannel = GetTextChannelByName(guild, channelName);

        if (existingChannel is not null)
        {
            return existingChannel;
        }
        return await guild.CreateTextChannelAsync(channelName);
    }
}