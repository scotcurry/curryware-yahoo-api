namespace curryware_yahoo_parsing_library.TransactionModels;

public class TransactionListWithCount
{
    public int TransactionCount { get; set; }
    public List<TransactionModel>? Transactions { get; set; }
}