namespace curryware_fantasy_command_line_tool.CommandLineHandlers;

public class InvalidOptionException : Exception
{
    public InvalidOptionException(string message) : base(message) { }
    
    public InvalidOptionException(string message, Exception innerException) : base(message, innerException) { }
}