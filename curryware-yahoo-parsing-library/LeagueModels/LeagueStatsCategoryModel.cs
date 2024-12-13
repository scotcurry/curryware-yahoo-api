namespace curryware_yahoo_parsing_library.LeagueModels;

public class LeagueStatsCategoryModel
{
    public long LeagueStatKey { get; set; }
    public int StatId { get; set; }
    public bool StatEnabled { get; set; }
    public string? StatName { get; set; }
    public string? StatDisplayName { get; set; }
    public string? StatGroup { get; set; }
    public string? StatAbbreviation { get; set; }
    public int? StatSortOrder { get; set; }
    public string? StatPositionType { get; set; }
    public int StatSortPostion { get; set; }
}