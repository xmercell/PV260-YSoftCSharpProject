using Discord.Rest;
using Discord.WebSocket;

namespace StockGrader.DiscordBot
{
    public static class Utils
    {
        public static SocketTextChannel GetTextChannelByName(SocketGuild guild, string channelName)
        {
            return guild.TextChannels.FirstOrDefault(c => c.Name.Equals(channelName, StringComparison.OrdinalIgnoreCase));
        }

        public static SocketRole GetRoleByName(SocketGuild guild, string roleName) 
        {
            return guild.Roles.FirstOrDefault(r => r.Name.Equals(roleName, StringComparison.Ordinal));
        }
    }
}
