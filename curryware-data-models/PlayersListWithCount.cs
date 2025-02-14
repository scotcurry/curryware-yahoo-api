namespace curryware_data_models;

public class PlayersListWithCount
{
    public int PlayerCount { get; set; }
    public List<PlayerModel>? Players { get; set; }
    public string? OAuthToken { get; set; }
}