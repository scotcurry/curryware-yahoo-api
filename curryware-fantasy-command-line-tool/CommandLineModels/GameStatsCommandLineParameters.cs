namespace curryware_kafka_command_line.CommandLineModels;

public class GameStatsCommandLineParameters
{
    public int LeagueId { get; set; }
    public int GameId { get; set; }
    public int Week { get; set; }
    public string? PlayerPosition { get; set; }
}