using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;
using static StockGrader.DiscordBot.Utils;

namespace StockGrader.DiscordBot.Modules
{
    public class SubscriptionModule : ModuleBase<SocketCommandContext>
    {
        [Command("daily")]
        public async Task DailyAsync()
        {
            await AssignUserToChannelAsync("daily");
        }

        [Command("weekly")]
        public async Task WeeklyAsync()
        {
            await AssignUserToChannelAsync("weekly");
        }

        [Command("biweekly")]
        public async Task BiWeeklyAsync()
        {
            await AssignUserToChannelAsync("biweekly");
        }

        [Command("monthly")]
        public async Task MonthlyAsync()
        {
            await AssignUserToChannelAsync("monthly");
        }

        private async Task AssignUserToChannelAsync(string channelName)
        {
            var user = Context.User;
            var channel = await GetOrCreateChannelAsync(channelName);

            if (channel is not null)
            { 
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

    }
}
