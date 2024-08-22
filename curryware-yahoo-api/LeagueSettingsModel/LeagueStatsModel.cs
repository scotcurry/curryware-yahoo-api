namespace curryware_yahoo_api.LeagueSettingsModel;

public class LeagueStatsModel
{
    public string? StatLeagueKey { get; set; }
    public int StatId { get; set; }
    public int StatEnabled { get; set; }
    public string? StatName { get; set; }
    public string? StatGroup { get; set; }
    public string? PositionType { get; set; }
    public decimal StatValue { get; set; }
}