namespace StockGrader.DiscordBot;

public class ReportPeriod
{
    public TimeSpan Duration { get; }

    public String Description { get; }

    public ReportPeriod(TimeSpan duration, string description)
    {
        Duration = duration;
        Description = description;
    }
}