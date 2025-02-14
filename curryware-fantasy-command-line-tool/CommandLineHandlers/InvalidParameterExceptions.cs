namespace curryware_fantasy_command_line_tool.CommandLineHandlers;

public class InvalidParameterException : Exception
{
    public InvalidParameterException(string message) : base(message) { }
    
    public InvalidParameterException(string message, Exception innerException) : base(message, innerException) { }
}