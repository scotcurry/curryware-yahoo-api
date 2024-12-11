namespace curryware_yahoo_parsing_library.LeagueModels;

public class LeagueInformationModel
{
    public string? LeagueKey { get; set;}
    public int LeagueId { get; set;}
    public string? LeagueName { get; set;}
    public string? LeagueUrl { get; set;}
    public string? DraftStatus { get; set;}
    public int NumTeams { get; set;}
    public Int64 LeagueUpdateTimeStamp { get; set;}
    public string? ScoringType { get; set;}
    public int CurrentWeek { get; set;}
    public int StartWeek { get; set;}
    public int EndWeek { get; set;}
    public bool IsFinished { get; set;}
    public int Season { get; set; }
    public string? RenewCode { get; set; }
    public bool AllowDisableList { get; set; }
}   