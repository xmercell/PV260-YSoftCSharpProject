using Discord.WebSocket;
using Discord;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using static StockGrader.DiscordBot.Utils;
using StockGrader.BL.Services;
using StockGrader.BL.Model;
using System.Text;

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
            Console.WriteLine("Timer_Elapsed");
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
                var embed = CreateEmbedFromDiff(diff,"Daily diff", "Change since yesterday");

                await dailyChannel.SendMessageAsync(embed: embed);
            }
        }

        private async Task CheckAndSendWeeklyMessageAsync(ITextChannel weeklyChannel)
        {
            var lastSentMessage = await GetLastSentMessageByBotAsync(weeklyChannel);
            if (lastSentMessage == null || DateTimeOffset.UtcNow - lastSentMessage.Value > TimeSpan.FromDays(7))
            {
                var diff = _diffManager.GetWeeklyDiff();
                var embed = CreateEmbedFromDiff(diff, "Weekly diff", "Change since last week");

                await weeklyChannel.SendMessageAsync(embed: embed);
            }
        }

        private async Task CheckAndSendBiweeklyMessageAsync(ITextChannel biweeklyChannel)
        {
            var lastSentMessage = await GetLastSentMessageByBotAsync(biweeklyChannel);
            if (lastSentMessage == null || DateTimeOffset.UtcNow - lastSentMessage.Value > TimeSpan.FromDays(14))
            {
                var diff = _diffManager.GetBiweeklyDiff();
                var embed = CreateEmbedFromDiff(diff, "Biweekly diff", "Change since two weeks ago");

                await biweeklyChannel.SendMessageAsync(embed: embed);
            }
        }

        private async Task CheckAndSendMonthlyMessageAsync(ITextChannel monthlyChannel)
        {
            var lastSentMessage = await GetLastSentMessageByBotAsync(monthlyChannel);
            if (lastSentMessage == null || DateTimeOffset.UtcNow - lastSentMessage.Value > TimeSpan.FromDays(30))
            {
                var diff = _diffManager.GetMotnhlyDiff();
                var embed = CreateEmbedFromDiff(diff, "Monthly diff", "Change since last month");

                await monthlyChannel.SendMessageAsync(embed: embed);
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

        private Embed CreateEmbedFromDiff(Diff diff,string title, string description)
        {
            var newPositionLines = GeneratePositionsContent((IList<AbstractPosition>)diff.NewPositions);
            var increasedPositions = GeneratePositionsContent((IList<AbstractPosition>)diff.IncreasedPositions);
            var reducedPositions = GeneratePositionsContent((IList<AbstractPosition>)diff.ReducedPositions);
            var unchangedPositions = GeneratePositionsContent((IList<AbstractPosition>)diff.UnchangedPositions);
            var removedPositions = GeneratePositionsContent((IList<AbstractPosition>)diff.RemovedPositions);
            var embed = new EmbedBuilder()
                    .WithTitle(title)
                    .WithDescription(description)
                    .AddField("New positions (Company Name, Ticker, #Shares, Weight(%))", newPositionLines)
                    .AddField("Increased positions (Company Name, Ticker, #Shares( 🔺 x%), Weight(%))", increasedPositions)
                    .AddField("Reduced positions (Company Name, Ticker, #Shares( 🔻 x%), Weight(%))", reducedPositions)
                    .AddField("Unchanged positions (Company Name, Ticker, #Shares, Weight(%))", unchangedPositions)
                    .AddField("Removed positions (Company Name, Ticker)", removedPositions)
                    .WithColor(Color.Blue)
                    .WithCurrentTimestamp()
                    .Build();
            return embed;

        }

        private string GeneratePositionsContent(IList<AbstractPosition> positions)
        {
            var result = new StringBuilder();
            foreach (var pos in positions)
            {
                result.AppendLine(pos.ToString());

            }
            return result.ToString();
        }

    }
}
