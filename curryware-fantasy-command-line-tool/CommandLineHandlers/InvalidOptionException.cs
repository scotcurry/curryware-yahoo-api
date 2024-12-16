namespace curryware_kafka_command_line.CommandLineHandlers;

public class InvalidOptionException : Exception
{
    public InvalidOptionException(string message) : base(message) { }
    
    public InvalidOptionException(string message, Exception innerException) : base(message, innerException) { }
}