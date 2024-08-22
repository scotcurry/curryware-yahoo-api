namespace curryware_yahoo_api.PlayerStatModels;

public class PlayerStatModel
{
    public string? PlayerKey { get; set; }
    
    public int PlayerId { get; set; }
    
    public List<StatValueModel>? StatValues { get; set; }
}

public class StatValueModel
{
    public int StatId { get; set; }
    
    public decimal StatValue { get; set; }
}