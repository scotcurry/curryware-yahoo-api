namespace curryware_fantasy_command_line_tool.CommandLineModels;

public class PlayerCommandLineParameters
{
    public int LeagueId { get; set; }
    public int GameId { get; set; }
    public string? PlayerPosition { get; set; }
    public string? PlayerStatus { get; set; }
}