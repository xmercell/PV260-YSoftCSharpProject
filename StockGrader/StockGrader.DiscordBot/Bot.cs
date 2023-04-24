using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using StockGrader.DiscordBot.Services;

namespace StockGrader.DiscordBot
{


    public class Bot
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commandService;
        private readonly IConfiguration _configuration;
        private ScheduledMessageService _scheduledMessageService;

        public Bot(IConfiguration configuration)
        {
            _configuration = configuration;
            

            var config = new DiscordSocketConfig
            {
                GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent
            };

            _client = new DiscordSocketClient(config);
            _commandService = new CommandService();
        }

        public async Task StartAsync()
        {
            await _client.LoginAsync(TokenType.Bot, _configuration["Token"]);
            await _client.StartAsync();

            _client.Log += LogAsync;
            _client.Ready += ReadyAsync;

            await _commandService.AddModulesAsync(Assembly.GetEntryAssembly(), null);

            _client.MessageReceived += HandleCommandAsync;
   
        }

        private async Task LogAsync(LogMessage log)
        {
            Console.WriteLine(log.ToString());
        }

        private async Task ReadyAsync()
        {
            Console.WriteLine($"{_client.CurrentUser} is connected!");
            _scheduledMessageService = new ScheduledMessageService(_client);
        }

        private async Task HandleCommandAsync(SocketMessage messageParam)
        {
            var message = messageParam as SocketUserMessage;
            if (message == null || message.Type != MessageType.Default || message.Author.IsBot || message.Author.IsWebhook) return;

            // Log the message content
            Console.WriteLine($"Message received from {message.Author.Username} ({message.Author.Id}): {message.Content} (Type: {message.Type})");

            int argPos = 0;
            char prefix = Char.Parse(_configuration["CommandPrefix"]);

            if (!(message.HasCharPrefix(prefix, ref argPos) || message.HasMentionPrefix(_client.CurrentUser, ref argPos))) return;

            var context = new SocketCommandContext(_client, message);
            await _commandService.ExecuteAsync(context, argPos, null);
        }
    }
}

