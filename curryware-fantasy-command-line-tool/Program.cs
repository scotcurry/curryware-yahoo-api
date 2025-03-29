using Microsoft.Extensions.Logging;

using curryware_fantasy_command_line_tool.CommandLineHandlers;
using curryware_fantasy_command_line_tool.CommandLineModels;
using curryware_fantasy_command_line_tool.PlayerCommands;
using curryware_fantasy_command_line_tool.StatsCommands;
using Serilog;

namespace curryware_fantasy_command_line_tool;

public abstract class Program
{
    public static async Task Main(string[] args)
    {
        try
        {
            var totalBatches = 0;
            var parsedCommandLineObject = CommandLineParser.ParseCommandLine(args);
            if (parsedCommandLineObject is PlayerCommandLineParameters parsedCommandLine)
                totalBatches = await PlayerCommand.RunGetPlayersCommand(parsedCommandLine);
            if (parsedCommandLineObject is GameStatsCommandLineParameters gameStatsCommandLine) 
            {
                // CurrywareLogHandler.AddLog($"Command Line: ", LogLevel.Debug);
                Log.Debug("Running stats command");
                var returnValue = await StatsCommand.GetStats(gameStatsCommandLine);
                totalBatches = returnValue.Count;
            }
            // CurrywareLogHandler.AddLog($"Wrote {totalBatches} to Kafka queue", LogLevel.Debug);
            Log.Debug($"Wrote {totalBatches} to Kafka queue");
        }
        catch (InvalidParameterException invalidParameterException)
        {
            // CurrywareLogHandler.AddLog(invalidParameterException.Message, LogLevel.Error);
            Log.Error(invalidParameterException.Message);
            PrintHelp();
            Environment.Exit(120);
        }
        catch (InvalidOperationException invalidOperationException)
        {
            // CurrywareLogHandler.AddLog(invalidOperationException.Message, LogLevel.Error);
            Log.Error(invalidOperationException.Message);
            PrintHelp();
            Environment.Exit(130);
        }
    }
    
    private static void PrintHelp()
    {
        Console.WriteLine("Usage: curryware-fantasy-command-line-tool <stats> | <players> [options]");
        Console.WriteLine("\n");
        Console.WriteLine("Game Statistics:");
        Console.WriteLine("\tstats");
        Console.WriteLine("\t-g <gameID>");
        Console.WriteLine("\t [-w <week>]");
        Console.WriteLine("\t [-P <position>] [QB, RB, WR, TE, K, D] - Only one position can be provided.");
        Console.WriteLine("\n");
        Console.WriteLine("Player Information:");
        Console.WriteLine("\tplayers");
        Console.WriteLine("\t [-P <position>] [QB, RB, WR, TE, K, D] - Only one position can be provided.");
        Console.WriteLine("\t [-s <status>] [A, FA, W, T] - Only one status can be provided.");
        Console.WriteLine("\t\t A - Available");
        Console.WriteLine("\t\t FA - Free Agent");
        Console.WriteLine("\t\t W - Waivers");
        Console.WriteLine("\t\t T - Taken");
    }
}
