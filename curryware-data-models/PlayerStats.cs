namespace curryware_data_models;

public class PlayerStats
{
    public int PlayerId { get; set; }
    public int GameKey { get; set; }
    public int WeekKey { get; set; }
    public int StatId { get; set; }
    public decimal StatValue { get; set; }
}