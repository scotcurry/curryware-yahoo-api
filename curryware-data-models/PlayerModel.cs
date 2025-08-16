namespace curryware_data_models;

public class PlayerModel
{
    public int Id { get; set; }
    public string? Key { get; set; }
    public string? FullName { get; set; }
    public string? Status { get; set; }
    public string? PlayerStatusFull { get; set; }
    public string? Url { get; set; }
    public string? Team { get; set; }
    public int ByeWeek { get; set; }
    public int UniformNumber { get; set; }
    public string? Position { get; set; }
    public string? Headshot { get; set; }
    public string? InjuryNotes { get; set; }
}