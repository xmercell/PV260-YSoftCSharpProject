using System.Collections.ObjectModel;
using Discord;
using Discord.WebSocket;

namespace StockGrader.DiscordBot.Extensions;

public static class SocketGuildExtension
{
    public static readonly IList<String> ChannelNames =
        new ReadOnlyCollection<string>(new List<String> { "daily", "weekly", "biweekly", "monthly" });


    public static SocketRole GetRoleByName(this SocketGuild guild, string roleName)
    {
        return guild.Roles.FirstOrDefault(r => r.Name.Equals(roleName, StringComparison.Ordinal));
    }

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

    public static IList<ulong> GetChannelRolesIDs(this SocketGuild guild)
    {
        List<ulong> ids = new();
        foreach (var role in guild.Roles)
        {
            if (ChannelNames.Contains(role.Name))
            {
                ids.Add(role.Id);
            }
        }

        return ids;
    }

    public static async Task<ITextChannel> EnsureRoleChannelExistsAsync(this SocketGuild guild, string channelName)
    {
        ITextChannel channel = guild.GetTextChannelByName(channelName);

        var role = await guild.GetOrCreateRoleAsync(channelName);

        if (channel is null)
        {
            channel = await guild.CreateTextChannelAsync(channelName);
        }

        var rolePermissions = channel.GetPermissionOverwrite(role);

        if (rolePermissions is null)
        {
            await guild.SetPrivateChannelPermissions(role, channel);
        }

        return channel;
    }

    public static async Task<IRole> GetOrCreateRoleAsync(this SocketGuild guild, string roleName)
    {
        var role = guild.GetRoleByName(roleName);

        if (role is not null)
        {
            return role;
        }

        return await guild.CreateRoleAsync(roleName);
    }

    
    private static async Task SetPrivateChannelPermissions(this SocketGuild guild, IRole role, ITextChannel channel)
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
}