namespace curryware_postgres_library.Models.PlayerModels;

public class PlayerModel
{
    public string? Key { get; set; }
    public int Id { get; set; }
    public string? FullName { get; set; }
    public string? Url { get; set; }
    public string? Status { get; set; }
    public string? Team { get; set; }
    public int? ByeWeek { get; set; }
    public int? UniformNumber { get; set; }
    public string? Position { get; set; }
    public string? Headshot { get; set; }
}