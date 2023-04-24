using Discord.WebSocket;

namespace StockGrader.DiscordBot
{
    public static class Utils
    {
        public static SocketTextChannel GetTextChannelByName(SocketGuild guild, string channelName)
        {
            return guild.TextChannels.FirstOrDefault(c => c.Name.Equals(channelName, StringComparison.OrdinalIgnoreCase));
        }
    }
}
