namespace curryware_kafka_producer_library;

public class KafkaValidationException : Exception
{
    public KafkaValidationException(string message) : base(message) { }
    
    public KafkaValidationException(string message, Exception innerException) : base(message, innerException) { }
}