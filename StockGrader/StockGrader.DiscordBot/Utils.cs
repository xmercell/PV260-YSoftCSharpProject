using Discord;
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

        public static async Task<ITextChannel> EnsureChannelExistsAsync(string channelName, SocketGuild guild, IRole role)
        {
            var existingChannel = GetTextChannelByName(guild, channelName);

            if (existingChannel is not null)
            {
                return existingChannel;
            }

            if (role == null) 
            {
                role = await GetOrCreateRoleAsync(channelName, guild);
            }
            //var role = await GetOrCreateRoleAsync(channelName, guild);

            var channel = await guild.CreateTextChannelAsync(channelName);
            var permissionOverrides = new OverwritePermissions(viewChannel: PermValue.Deny);
            var perrmisionChannelRole = new OverwritePermissions(viewChannel: PermValue.Allow);

            await channel.AddPermissionOverwriteAsync(
                role,
                OverwritePermissions
                .DenyAll(channel)
                .Modify(viewChannel: PermValue.Allow)
                .Modify(readMessageHistory: PermValue.Allow)
            );

            await channel.AddPermissionOverwriteAsync(
                guild.EveryoneRole,
                 OverwritePermissions.DenyAll(channel)
                );
            return channel;
        }

        public static async Task<IRole> GetOrCreateRoleAsync(string roleName, SocketGuild guild)
        {
            var role = GetRoleByName(guild, roleName);

            if (role is not null)
            {
                return role;
            }

            return await guild.CreateRoleAsync(roleName);
        }
    }
}
