namespace curryware_yahoo_parsing_library.TransactionModels;

public class TransactionModel
{
    public string? TransactionKey { get; set; }
    public int TransactionId { get; set; }
    public string? TransactionType { get; set; }
    public string? TransactionStatus { get; set; }
    public int? TransactionTimestamp { get; set; }
    
    public List<TransactionPlayersInvolved>? PlayersInvolved { get; set; }
}