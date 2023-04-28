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
            var role = await GetOrCreateRoleAsync(channelName, Context.Guild);

            if (role == null)
            {
                await Context.Channel.SendMessageAsync($"Unable to find or create the {channelName} role.");
                // TODO: handle early return
                return;
            }

            var channel = await EnsureRoleChannelExistsAsync(channelName, Context.Guild);
            

            if (channel is not null)
            {
                var guildUser = user as IGuildUser;
                var roleIDs = await GetChannelRolesIDs(Context.Guild);
                await guildUser.RemoveRolesAsync(roleIDs);
                await (Context.User as IGuildUser).AddRoleAsync(role);
                await Context.Channel.SendMessageAsync($"{user.Mention} has been assigned to {channel.Mention}");
            }
            else
            {
                await Context.Channel.SendMessageAsync($"Unable to find or create the {channelName} channel.");
            }
        }
    }
}
