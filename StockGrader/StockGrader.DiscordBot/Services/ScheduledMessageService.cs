using Discord.WebSocket;
using Discord;
using System.Timers;
using static StockGrader.DiscordBot.Utils;
using StockGrader.BL.Services;
using StockGrader.BL.Model;
using System.Text;
using StockGrader.DiscordBot.Extensions;

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
                await guild.EnsureChannelExistsAsync("subs");
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
                Diff diff;
                try
                {
                    diff = _diffManager.GetDailyDiff();
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return;
                }

                var embed = BuildEmbedFromDiff(diff,"Daily diff", "Change since yesterday");

                await dailyChannel.SendMessageAsync(embed: embed);
            }
        }

        private async Task CheckAndSendWeeklyMessageAsync(ITextChannel weeklyChannel)
        {
            var lastSentMessage = await GetLastSentMessageByBotAsync(weeklyChannel);
            if (lastSentMessage == null || DateTimeOffset.UtcNow - lastSentMessage.Value > TimeSpan.FromDays(7))
            { 
                Diff diff;
                try
                {
                    diff = _diffManager.GetWeeklyDiff();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return;
                }
                var embed = BuildEmbedFromDiff(diff, "Weekly diff", "Change since last week");

                await weeklyChannel.SendMessageAsync(embed: embed);
            }
        }

        private async Task CheckAndSendBiweeklyMessageAsync(ITextChannel biweeklyChannel)
        {
            var lastSentMessage = await GetLastSentMessageByBotAsync(biweeklyChannel);
            if (lastSentMessage == null || DateTimeOffset.UtcNow - lastSentMessage.Value > TimeSpan.FromDays(14))
            {
                Diff diff;
                try
                {
                    diff = _diffManager.GetBiweeklyDiff();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return;
                }
                var embed = BuildEmbedFromDiff(diff, "Biweekly diff", "Change since two weeks ago");

                await biweeklyChannel.SendMessageAsync(embed: embed);
            }
        }

        private async Task CheckAndSendMonthlyMessageAsync(ITextChannel monthlyChannel)
        {
            var lastSentMessage = await GetLastSentMessageByBotAsync(monthlyChannel);
            if (lastSentMessage == null || DateTimeOffset.UtcNow - lastSentMessage.Value > TimeSpan.FromDays(30))
            {
                Diff diff;
                try
                {
                    diff = _diffManager.GetMotnhlyDiff();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return;
                }
                var embed = BuildEmbedFromDiff(diff, "Monthly diff", "Change since last month");

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

        private Embed BuildEmbedFromDiff(Diff diff,string title, string description)
        {
            var newPositionLines = GeneratePositionsContent(diff.NewPositions);
            var increasedPositions = GeneratePositionsContent(diff.IncreasedPositions);
            var reducedPositions = GeneratePositionsContent(diff.ReducedPositions);
            var unchangedPositions = GeneratePositionsContent(diff.UnchangedPositions);
            var removedPositions = GeneratePositionsContent(diff.RemovedPositions);

            var builder = new StringBuilder();

            builder
                .AppendLine(description)
                .AppendLine()
                .AppendLine("**New positions**")
                .AppendLine("** Company Name, Ticker, #Shares, Weight(%) **")
                .AppendLine()
                .AppendLine(newPositionLines)
                .AppendLine()
                .AppendLine("**Increased positions**")
                .AppendLine("** Company Name, Ticker, #Shares( 🔺 x%), Weight(%) **")
                .AppendLine()
                .AppendLine(increasedPositions)
                .AppendLine()
                .AppendLine("**Reduced positions**")
                .AppendLine("** Company Name, Ticker, #Shares( 🔻 x%), Weight(%) **")
                .AppendLine()
                .AppendLine(reducedPositions)
                .AppendLine()
                .AppendLine("**Unchanged positions**")
                .AppendLine("** Company Name, Ticker, #Shares, Weight(%) **")
                .AppendLine()
                .AppendLine(unchangedPositions)
                .AppendLine()
                .AppendLine("**Removed positions**")
                .AppendLine("** Company Name, Ticker **")
                .AppendLine()
                .AppendLine(removedPositions);


            var embed = new EmbedBuilder()
                    .WithTitle(title)
                    .WithDescription(builder.ToString())      
                    .WithColor(Color.Blue)
                    .WithCurrentTimestamp()
                    .Build();
            return embed;
        }

        private string GeneratePositionsContent(IEnumerable<IPosition> positions)
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
