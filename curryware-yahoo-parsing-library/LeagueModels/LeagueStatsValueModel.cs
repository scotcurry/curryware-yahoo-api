namespace curryware_yahoo_parsing_library.LeagueModels;

public class LeagueStatsValueModel
{
    public Int64 LeagueStatKey { get; set; }
    public int GameId { get; set; }
    
    public int LeagueId { get; set; }
    public int StatId { get; set; }
    public decimal? StatValue { get; set; }
}