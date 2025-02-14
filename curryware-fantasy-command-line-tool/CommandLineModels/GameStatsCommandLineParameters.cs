namespace curryware_fantasy_command_line_tool.CommandLineModels;

public class GameStatsCommandLineParameters
{
    public int LeagueId { get; set; }
    public int GameId { get; set; }
    public int Week { get; set; }
    public string? PlayerPosition { get; set; }
}