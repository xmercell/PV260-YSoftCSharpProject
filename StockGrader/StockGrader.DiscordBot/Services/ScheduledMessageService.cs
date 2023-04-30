using Discord.WebSocket;
using Discord;
using System.Timers;
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
        private static readonly int WeekDayCount = 7;
        private static readonly int MonthDayCount = 30; 
        private List<ReportPeriod> _reportPeriods;

        public ScheduledMessageService(DiscordSocketClient client, IDiffManager diffManager)
        {
            InitReportPeriods();
            _client = client;
            _diffManager = diffManager;

            _timer = new System.Timers.Timer(60000); // 60,000 milliseconds = 1 minute
            _timer.Elapsed += Timer_Elapsed;
            _timer.AutoReset = true;
            _timer.Start();

            Timer_Elapsed(null, null);
        }

        private void InitReportPeriods()
        {
            _reportPeriods = new();
            _reportPeriods.Add(new ReportPeriod(TimeSpan.FromDays(1), "daily"));
            _reportPeriods.Add(new ReportPeriod(TimeSpan.FromDays(WeekDayCount), "weekly"));
            _reportPeriods.Add(new ReportPeriod(TimeSpan.FromDays(2 * WeekDayCount), "biweekly"));
            _reportPeriods.Add(new ReportPeriod(TimeSpan.FromDays(MonthDayCount), "monthly"));
        }

        private async Task ServeGuild(SocketGuild guild)
        {
            await guild.EnsureChannelExistsAsync("subs");
                
            List<(ITextChannel, ReportPeriod)> channels = new();
            await Parallel.ForEachAsync(_reportPeriods,
                async (reportPeriod, _) =>
                {
                    channels.Add((await guild.EnsureRoleChannelExistsAsync(reportPeriod.Description), reportPeriod));
                });

            foreach (var (textChannel, reportPeriod) in channels)
            {
                await CheckAndSendMessageAsync(textChannel, reportPeriod);
            }
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
                await ServeGuild(guild);
            }
        }

        private bool IsTimeToSendMessage(DateTimeOffset? lastSentMessage, ReportPeriod reportPeriod)
        {
            return lastSentMessage == null || DateTimeOffset.UtcNow - lastSentMessage.Value > reportPeriod.Duration;
        }
        
        private async Task CheckAndSendMessageAsync(ITextChannel dailyChannel, ReportPeriod reportPeriod)
        {
            var lastSentMessage = await GetLastSentMessageByBotAsync(dailyChannel);
            if (IsTimeToSendMessage(lastSentMessage, reportPeriod))
            {
                Diff diff;
                try
                {
                    diff = _diffManager.GetDiffSince(GetTimeAgo(reportPeriod));
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return;
                }

                var embed = BuildEmbedFromDiff(diff,$"{reportPeriod.Description} diff", $"Change from {reportPeriod.Duration.Days} days ago.");

                await dailyChannel.SendMessageAsync(embed: embed);
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

        private DateTime GetTimeAgo(ReportPeriod reportPeriod)
        {
            return DateTime.Now.AddDays(reportPeriod.Duration.Negate().Days);
        }
    }
}
