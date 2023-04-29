using Discord.WebSocket;
using Discord;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using static StockGrader.DiscordBot.Utils;
using StockGrader.BL.Services;

namespace StockGrader.DiscordBot.Services
{
    public class ScheduledMessageService
    {
        private readonly DiscordSocketClient _client;
        private readonly System.Timers.Timer _timer;
        private readonly IDiffManager _diffManager;

        public ScheduledMessageService(DiscordSocketClient client, IDiffManager diffManager)
        {
            _client = client;
            _diffManager = diffManager;

            _timer = new System.Timers.Timer(60000); // 60,000 milliseconds = 1 minute
            _timer.Elapsed += Timer_Elapsed;
            _timer.AutoReset = true;
            _timer.Start();

            Timer_Elapsed(null, null);
        }

        private async void Timer_Elapsed(object? sender, ElapsedEventArgs? e)
        {
            Console.WriteLine("Timer");
            if (_client.ConnectionState != ConnectionState.Connected)
            {
                return;
            }

            foreach (var guild in _client.Guilds)
            {
                await EnsureChannelExistsAsync("subs", guild);
                var dailyChannel = await EnsureRoleChannelExistsAsync("daily", guild);
                var weeklyChannel = await EnsureRoleChannelExistsAsync("weekly", guild);
                var biweeklyChannel = await EnsureRoleChannelExistsAsync("biweekly", guild);
                var monthlyChannel = await EnsureRoleChannelExistsAsync("monthly", guild);

                await CheckAndSendDailyMessageAsync(dailyChannel);
                await CheckAndSendWeeklyMessageAsync(weeklyChannel);
                await CheckAndSendBiweeklyMessageAsync(biweeklyChannel);
                await CheckAndSendMonthlyMessageAsync(monthlyChannel);
            }
        }

        private async Task CheckAndSendDailyMessageAsync(ITextChannel dailyChannel)
        {
            var lastSentMessage = await GetLastSentMessageByBotAsync(dailyChannel);
            if (lastSentMessage == null || DateTimeOffset.UtcNow - lastSentMessage.Value > TimeSpan.FromDays(1))
            {
                var diff = _diffManager.GetDailyDiff();
                var embed = new EmbedBuilder()
                                    .WithTitle("Daily comparison")
                                    .WithDescription("Shows changes in positions since yesterday")
                                    .AddField()


                await dailyChannel.SendMessageAsync("Daily message!");
            }
        }

        private async Task CheckAndSendWeeklyMessageAsync(ITextChannel weeklyChannel)
        {
            var lastSentMessage = await GetLastSentMessageByBotAsync(weeklyChannel);
            if (lastSentMessage == null || DateTimeOffset.UtcNow - lastSentMessage.Value > TimeSpan.FromDays(7))
            {
                await weeklyChannel.SendMessageAsync("Weekly message!");
            }
        }

        private async Task CheckAndSendBiweeklyMessageAsync(ITextChannel biweeklyChannel)
        {
            var lastSentMessage = await GetLastSentMessageByBotAsync(biweeklyChannel);
            if (lastSentMessage == null || DateTimeOffset.UtcNow - lastSentMessage.Value > TimeSpan.FromDays(14))
            {
                await biweeklyChannel.SendMessageAsync("Biweekly message!");
            }
        }

        private async Task CheckAndSendMonthlyMessageAsync(ITextChannel monthlyChannel)
        {
            var lastSentMessage = await GetLastSentMessageByBotAsync(monthlyChannel);
            if (lastSentMessage == null || DateTimeOffset.UtcNow - lastSentMessage.Value > TimeSpan.FromDays(30))
            {
                await monthlyChannel.SendMessageAsync("Monthly message!");
            }
        }

        private async Task<DateTimeOffset?> GetLastSentMessageByBotAsync(ITextChannel channel)
        {
            var messages = await channel.GetMessagesAsync(100).FlattenAsync();

            foreach (var message in messages)
            {
                if (message.Author.Id == _client.CurrentUser.Id)
                {
                    return message.Timestamp;
                }
            }

            return null;
        }
    }
}
