namespace curryware_yahoo_api.KafkaHandlers;

public class KafkaValidationException : Exception
{
    public KafkaValidationException(string message) : base(message) { }
    
    public KafkaValidationException(string message, Exception innerException) : base(message, innerException) { }
}