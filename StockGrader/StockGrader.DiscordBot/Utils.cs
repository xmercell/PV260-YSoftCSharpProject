using Discord;
using Discord.Rest;
using Discord.WebSocket;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Channels;

namespace StockGrader.DiscordBot
{
    public static class Utils
    {
        public static readonly IList<String> ChannelNames = new ReadOnlyCollection<string>(new List<String> {"daily", "weekly", "biweekly", "monthly" });
        public static SocketTextChannel GetTextChannelByName(SocketGuild guild, string channelName)
        {
            return guild.TextChannels.FirstOrDefault(c => c.Name.Equals(channelName, StringComparison.OrdinalIgnoreCase));
        }

        public static SocketRole GetRoleByName(SocketGuild guild, string roleName) 
        {
            return guild.Roles.FirstOrDefault(r => r.Name.Equals(roleName, StringComparison.Ordinal));
        }

        public static async Task<ITextChannel> EnsureChannelExistsAsync(string channelName, SocketGuild guild)
        {
            var existingChannel = GetTextChannelByName(guild, channelName);

            if (existingChannel is not null)
            {
                return existingChannel;
            }
            return await guild.CreateTextChannelAsync(channelName);
        }

        public static async Task<ITextChannel> EnsureRoleChannelExistsAsync(string channelName, SocketGuild guild)
        {
            ITextChannel channel = GetTextChannelByName(guild, channelName);

            var role = await GetOrCreateRoleAsync(channelName, guild);

            if (channel is null)
            {
                channel = await guild.CreateTextChannelAsync(channelName);
            }

            var rolePermissions = channel.GetPermissionOverwrite(role);

            if (rolePermissions is null)
            {
                await SetPrivateChannelPermissions(guild, role, channel);
            }
            return channel;
        }

        private static async Task SetPrivateChannelPermissions(SocketGuild guild, IRole role, ITextChannel channel)
        {
            await channel.AddPermissionOverwriteAsync(
                            role,
                            OverwritePermissions
                                .DenyAll(channel)
                                .Modify(viewChannel: PermValue.Allow)
                                .Modify(readMessageHistory: PermValue.Allow)
                        );

            await channel.AddPermissionOverwriteAsync(
                            guild.EveryoneRole,
                            OverwritePermissions
                                .DenyAll(channel)
                );
        }

        public static async Task<ITextChannel> GetOrCreateChannelAsync(string channelName, SocketGuild guild)
        {
            var channel = GetTextChannelByName(guild, channelName);

            if (channel is not null)
            {
                return channel;
            }

            return await guild.CreateTextChannelAsync(channelName);
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

        public static async Task<IList<ulong>> GetChannelRolesIDs(SocketGuild guild) 
        { 
            var roleIDs = new List<ulong>();
            foreach (var role in guild.Roles)
            {
                if (ChannelNames.Contains(role.Name))
                {
                    roleIDs.Add(role.Id);
                }
            }

            return roleIDs;
        }
    }
}
