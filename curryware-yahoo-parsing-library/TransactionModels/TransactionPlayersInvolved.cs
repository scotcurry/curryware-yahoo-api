namespace curryware_yahoo_parsing_library.TransactionModels;

public class TransactionPlayersInvolved
{
    public string? PlayerKey { get; set; }
    public int PlayerId { get; set; }
    public string? TransactionType { get; set; }
    public string? TransactionSource { get; set; }
    public string? DestinationType { get; set; }
    public string? DestinationTeamId { get; set; }
}