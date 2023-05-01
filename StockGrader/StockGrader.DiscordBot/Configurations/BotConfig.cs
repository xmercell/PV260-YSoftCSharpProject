
namespace StockGrader.DiscordBot.Configurations
{
    public class BotConfig
    {
        public string Token { get; init; }
        public string Prefix { get; init; }

        public BotConfig(string token, string prefix) 
        {
            Token = token;
            Prefix = prefix;
        }
    }
}
