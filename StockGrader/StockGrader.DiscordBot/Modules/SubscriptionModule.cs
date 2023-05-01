using Discord;
using Discord.Commands;
using Discord.Rest;
using Discord.WebSocket;
using StockGrader.DiscordBot.Prerequisities;
using System.Text;
using System.Threading.Tasks;
using StockGrader.DiscordBot.Extensions;

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

        [RequireChannel("subs")]
        [Command("help")]
        public async Task HelpAsync()
        {
            var channel = Context.Channel;
            var builder = new StringBuilder();
            builder
                .AppendLine("**Commands for signing to channels**")
                .AppendLine("*daily* for daily stock changes channel")
                .AppendLine("*weekly* for weekly stock changes channel")
                .AppendLine("*biweekly* for biweekly stock changes channel")
                .AppendLine("*monthly* for monthly stock changes channel");

            await channel.SendMessageAsync(builder.ToString());
        }

        private async Task<IGuildUser> FetchUser(ulong userId)
        {
            var users = await Context.Guild.GetUsersAsync(new RequestOptions()).FlattenAsync();
            return users.FirstOrDefault(user => user.Id == userId);
        }
        
        private async Task AssignUserToChannelAsync(string channelName)
        {

            // roles can be named the same way as channels, or we can add parameter
            var role = await Context.Guild.GetOrCreateRoleAsync(channelName);

            if (role == null)
            {
                await Context.Channel.SendMessageAsync($"Unable to find or create the {channelName} role.");
                return;
            }

            var channel = await Context.Guild.EnsureRoleChannelExistsAsync(channelName);
            

            if (channel is not null)
            {
                var roleIds = Context.Guild.GetChannelRolesIDs();
                
                await (Context.User as IGuildUser).RemoveRolesAsync(roleIds);
                // Important: Due to the library caching, we have to re-fetch the user
                var reFetchedUser = await FetchUser(Context.User.Id);
                await reFetchedUser.AddRoleAsync(role);

                await Context.Channel.SendMessageAsync($"{Context.User.Mention} has been assigned to {channel.Mention}");
            }
            else
            {
                await Context.Channel.SendMessageAsync($"Unable to find or create the {channelName} channel.");
            }
        }
    }
}
