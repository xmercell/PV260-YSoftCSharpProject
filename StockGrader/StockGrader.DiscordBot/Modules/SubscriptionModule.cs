using Discord;
using Discord.Commands;
using Discord.Rest;
using Discord.WebSocket;
using StockGrader.DiscordBot.Prerequisities;
using System.Threading.Tasks;
using static StockGrader.DiscordBot.Utils;

namespace StockGrader.DiscordBot.Modules
{
    public class SubscriptionModule : ModuleBase<SocketCommandContext>
    {
        [RequireChannel("subs")]
        [Command("daily")]
        public async Task DailyAsync()
        {
            await AssignUserToChannelAsync("daily");
        }

        [RequireChannel("subs")]
        [Command("weekly")]
        public async Task WeeklyAsync()
        {
            await AssignUserToChannelAsync("weekly");
        }

        [RequireChannel("subs")]
        [Command("biweekly")]
        public async Task BiWeeklyAsync()
        {
            await AssignUserToChannelAsync("biweekly");
        }

        [RequireChannel("subs")]
        [Command("monthly")]
        public async Task MonthlyAsync()
        {
            await AssignUserToChannelAsync("monthly");
        }

        private async Task AssignUserToChannelAsync(string channelName)
        {
            var user = Context.User;
            // roles can be named the same way as channels, or we can add parameter
            var role = await GetOrCreateRoleAsync(channelName);

            if (role == null)
            {
                await Context.Channel.SendMessageAsync($"Unable to find or create the {channelName} role.");
                // TODO: handle early return
                return;
            }

            var channel = await GetOrCreateRoleChannelAsync(channelName, role);
            

            if (channel is not null)
            {
                // can this really be null?
                await (user as IGuildUser).AddRoleAsync(role);
                await Context.Channel.SendMessageAsync($"{user.Mention} has been assigned to {channel.Mention}");
            }
            else
            {
                await Context.Channel.SendMessageAsync($"Unable to find or create the {channelName} channel.");
            }
        }

        private async Task<ITextChannel> GetOrCreateChannelAsync(string channelName)
        {
            var guild = Context.Guild;
            var existingChannel = GetTextChannelByName(guild, channelName);

            if (existingChannel is not null)
            {
                return existingChannel;
            }

           return await guild.CreateTextChannelAsync(channelName);
        }

        private async Task<ITextChannel> GetOrCreateRoleChannelAsync(string channelName, IRole role)
        {
            var guild = Context.Guild;
            var existingChannel = GetTextChannelByName(guild, channelName);

            if (existingChannel is not null)
            {
                return existingChannel;
            }

            var channel = await guild.CreateTextChannelAsync(channelName);
            await channel.AddPermissionOverwriteAsync(
                role, 
                OverwritePermissions
                    .DenyAll(channel)
                    .Modify(viewChannel: PermValue.Allow) 
            );
            await channel.AddPermissionOverwriteAsync(
                guild.EveryoneRole,
                OverwritePermissions
                    .DenyAll(channel)
                );
            return channel;
        }

        private async Task<IRole> GetOrCreateRoleAsync(string roleName)
        {
            var guild = Context.Guild;
            var role = GetRoleByName(guild, roleName);

            if (role is not null) 
            {
                return role;
            }

            return await guild.CreateRoleAsync(roleName);
        }

    }
}
