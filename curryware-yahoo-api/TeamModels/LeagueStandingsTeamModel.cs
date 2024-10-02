namespace curryware_yahoo_api.TeamModels;

public class LeagueStandingsTeamModel
{
    public string? TeamKey { get; set; }
    public string? TeamName { get; set; }
    public string? TeamLogo { get; set; }
    public string? ManagerNickName { get; set; }
    public string? ManagerImageUrl { get; set; }
    public int ManagerFeloScore { get; set; }
    public decimal TeamTotalPoints { get; set; }
    public int TeamRank { get; set; }
    public int TeamWins { get; set; }
    public int TeamLosses { get; set; }
    public int TeamTies { get; set; }
}