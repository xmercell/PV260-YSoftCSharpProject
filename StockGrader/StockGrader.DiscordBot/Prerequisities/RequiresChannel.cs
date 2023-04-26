using Discord.Commands;


namespace StockGrader.DiscordBot.Prerequisities
{
    public class RequireChannel : PreconditionAttribute
    {
        private readonly string _name;

        public RequireChannel(string name)
        {
            _name = name;
        }
        public override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
        {
            if (context.Channel.Name == _name)
            {
                return Task.FromResult(PreconditionResult.FromSuccess());
            }
            else
            {
                return Task.FromResult(PreconditionResult.FromError($"This command is usable only in {_name} channel," +
                    $" not in {context.Channel.Name}"));
            }  
        }
    }
}
