namespace curryware_kafka_command_line.CommandLineHandlers;

public class InvalidParameterException : Exception
{
    public InvalidParameterException(string message) : base(message) { }
    
    public InvalidParameterException(string message, Exception innerException) : base(message, innerException) { }
}