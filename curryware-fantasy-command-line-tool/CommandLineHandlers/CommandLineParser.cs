using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;

using curryware_fantasy_command_line_tool.CommandLineModels;
using Serilog;

namespace curryware_fantasy_command_line_tool.CommandLineHandlers;

public abstract class CommandLineParser
{
    public static object ParseCommandLine(string[] args)
    {
        object? returnObject = null;
        if (args.Contains("stats"))
            returnObject = ParseGameStatsCommandLine(args);
        if (args.Contains("players"))
            returnObject = ParsePlayerCommandLine(args);
        else if (returnObject == null)
            throw new InvalidParameterException("No Top Level Command Provided.");
        
        return returnObject;
    }

    public static GameStatsCommandLineParameters GameStatsParametersRest (Dictionary<string, string?> args)
    {
        var gameStatInformation = new GameStatsCommandLineParameters
        {
            LeagueId = Convert.ToInt32(args["leagueId"]),
            GameId = Convert.ToInt32(args["gameId"]),
            Week = Convert.ToInt32(args["week"]),
            PlayerPosition = args["position"] as string
        };
        
        return gameStatInformation;
    }

    public static PlayerCommandLineParameters PlayerLoadParametersRest(Dictionary<string, string?> args)
    {
        var leagueInformation = new PlayerCommandLineParameters
        {
            LeagueId = Convert.ToInt32(args["leagueId"]),
            GameId = Convert.ToInt32(args["gameId"]),
            PlayerPosition = args["position"] as string,
            PlayerStatus = args["status"] as string
        };
        
        return leagueInformation;
    }
    
    private static object ParsePlayerCommandLine(string[] args)
    {
        var playerCommandLineParameters = new PlayerCommandLineParameters
        {
            LeagueId = -1,
            GameId = -1,
            PlayerPosition = "None",
            PlayerStatus = "None"
        };
        
        var totalCommandLineLength = args.Length;
        var gameIdString = "-g";
        var leagueIdString = "-l";
        var positionString = "-P";
        var statusString = "-s";
        var gameIdIndex = Array.IndexOf(args, gameIdString);
        var leagueIdIndex = Array.IndexOf(args, leagueIdString);
        var positionIndex = Array.IndexOf(args, positionString);
        var statusIndex = Array.IndexOf(args, statusString);
        
        // Validate the game ID
        if (gameIdIndex != -1)
        {
            if (gameIdIndex == totalCommandLineLength - 1)
            {
                // CurrywareLogHandler.AddLog("Game ID was not provided.", LogLevel.Error);
                Log.Error("Game ID was not provided.");
                throw new InvalidParameterException("A gameID must be provided.");
            }
            
            var gameIdValue = args[gameIdIndex + 1];
            if (gameIdValue.Length < 1)
            {
                // CurrywareLogHandler.AddLog("Game ID was not provided.", LogLevel.Error);
                Log.Error("Game ID was not provided.");
                throw new InvalidParameterException("A gameID must be provided.");
            }
            else
            {
                gameIdValue = gameIdValue.Trim();
                string pattern = @"^\d{3}$";
                if (!Regex.IsMatch(gameIdValue, pattern))
                {
                    // CurrywareLogHandler.AddLog("Game ID was not provided.", LogLevel.Error);
                    Log.Error("Game ID was not provided."); ;
                    throw new InvalidParameterException("Game ID must be a 3 digits.");
                }

                playerCommandLineParameters.GameId = Convert.ToInt32(gameIdValue);
            }
        }
        
        // Validate the league ID
        if (leagueIdIndex != -1)
        {
            if (leagueIdIndex == totalCommandLineLength - 1)
            {
                // CurrywareLogHandler.AddLog("League ID was not provided.", LogLevel.Error);
                Log.Error("League ID was not provided.");
                throw new InvalidParameterException("A leagueId must be provided.");
            }
            
            var leagueIdValue = args[leagueIdIndex + 1];
            if (leagueIdValue.Length < 1)
            {
                // CurrywareLogHandler.AddLog("League ID was not provided.", LogLevel.Error);
                Log.Error("League ID was not provided.");
                throw new InvalidParameterException("A leagueId must be provided.");
            }
            else
            {
                leagueIdValue = leagueIdValue.Trim();
                var pattern = @"^\d{2,6}$";
                if (!Regex.IsMatch(leagueIdValue, pattern))
                {
                    // CurrywareLogHandler.AddLog("League ID was not provided.", LogLevel.Error);
                    Log.Error("League ID was not provided.");
                    throw new InvalidOptionException("League ID must be between 2 and 6 digits.");
                }

                playerCommandLineParameters.LeagueId = Convert.ToInt32(leagueIdValue);
            }
        }

        
        // All the player commands are optional.
        if (positionIndex != -1)
        {
            if (positionIndex == args.Length)
            {
                // CurrywareLogHandler.AddLog("Position was not provided. Must be [QB, RB, WR, TE, K, or D", LogLevel.Error);
                Log.Error("Position was not provided. Must be [QB, RB, WR, TE, K, or D");
                throw new InvalidOptionException("A position must be provided. Must be [QB, RB, WR, TE, K, or D");
            }

            var positionValue = args[positionIndex + 1].ToLower().Trim();
            switch (positionValue)
            {
                case "qb":
                    playerCommandLineParameters.PlayerPosition = "QB";
                    break;
                case "rb":
                    playerCommandLineParameters.PlayerPosition = "RB";
                    break;
                case "wr":
                    playerCommandLineParameters.PlayerPosition = "WR";
                    break;
                case "te":
                    playerCommandLineParameters.PlayerPosition = "TE";
                    break;
                case "k":
                    playerCommandLineParameters.PlayerPosition = "K";
                    break;
                default:
                    throw new InvalidOptionException("Position must be [QB, RB, WR, TE, K, or D]"); 
            }
        }

        if (statusIndex == -1) return playerCommandLineParameters;
        if (statusIndex == args.Length)
        {
            // CurrywareLogHandler.AddLog("Status was not provided. Must be [A, FA, W, or T", LogLevel.Error);
            Log.Error("Status was not provided. Must be [A, FA, W, or T");
            throw new InvalidOptionException("Status was not provided. Must be [A, FA, W, or T");
        }

        statusString = args[statusIndex + 1].ToLower().Trim();
        switch (statusString)
        {
            case "a":
                playerCommandLineParameters.PlayerStatus = "A";
                break;
            case "fa":
                playerCommandLineParameters.PlayerStatus = "FA";
                break;
            case "w":
                playerCommandLineParameters.PlayerStatus = "W";
                break;
            case "t":
                playerCommandLineParameters.PlayerStatus = "T";
                break;
            default:
                throw new InvalidOptionException("Status must be [A, FA, W, or T]");
        }
        return playerCommandLineParameters;
    }
    
    private static GameStatsCommandLineParameters ParseGameStatsCommandLine(string[] args)
    {
        var gameStatInformation = new GameStatsCommandLineParameters
        {
            LeagueId = -1,
            GameId = -1,
            Week = -1,
            PlayerPosition = "None"
        };
        
        var totalCommandLineLength = args.Length;
        var gameIdString = "-g";
        var leagueIdString = "-l";
        var weekIdString = "-w";
        var positionString = "-P";
        var gameIdIndex = Array.IndexOf(args, gameIdString);
        var leagueIdIndex = Array.IndexOf(args, leagueIdString);
        var weekIdIndex = Array.IndexOf(args, weekIdString);
        var positionIndex = Array.IndexOf(args, positionString);
        // Required parameters are gameID and leagueID.  Optional parameters are week to pull from and Position.
        
        // Validate the game ID
        if (gameIdIndex != -1)
        {
            if (gameIdIndex == totalCommandLineLength - 1)
            {
                // CurrywareLogHandler.AddLog("Game ID was not provided.", LogLevel.Error);
                Log.Error("Game ID was not provided.");
                throw new InvalidParameterException("A gameID must be provided.");
            }
            
            var gameIdValue = args[gameIdIndex + 1];
            if (gameIdValue.Length < 1)
            {
                // CurrywareLogHandler.AddLog("Game ID was not provided.", LogLevel.Error);
                Log.Error("Game ID was not provided.");
                throw new InvalidParameterException("A gameID must be provided.");
            }
            else
            {
                gameIdValue = gameIdValue.Trim();
                string pattern = @"^\d{3}$";
                if (!Regex.IsMatch(gameIdValue, pattern))
                {
                   // CurrywareLogHandler.AddLog("Game ID was not provided.", LogLevel.Error);
                   Log.Error("Game ID was not provided.");
                    throw new InvalidParameterException("Game ID must be a 3 digits.");
                }

                gameStatInformation.GameId = Convert.ToInt32(gameIdValue);
            }
        }
        
        // Validate the league ID
        if (leagueIdIndex != -1)
        {
            if (leagueIdIndex == totalCommandLineLength - 1)
            {
                // CurrywareLogHandler.AddLog("League ID was not provided.", LogLevel.Error);
                Log.Error("League ID was not provided.");
                throw new InvalidParameterException("A leagueId must be provided.");
            }
            
            var leagueIdValue = args[leagueIdIndex + 1];
            if (leagueIdValue.Length < 1)
            {
                // CurrywareLogHandler.AddLog("League ID was not provided.", LogLevel.Error);
                Log.Error("League ID was not provided.");
                throw new InvalidParameterException("A leagueId must be provided.");
            }
            else
            {
                leagueIdValue = leagueIdValue.Trim();
                var pattern = @"^\d{2,6}$";
                if (!Regex.IsMatch(leagueIdValue, pattern))
                {
                    // CurrywareLogHandler.AddLog("League ID was not provided.", LogLevel.Error);
                    Log.Error("League ID was not provided.");
                    throw new InvalidOptionException("League ID must be between 2 and 6 digits.");
                }

                gameStatInformation.LeagueId = Convert.ToInt32(leagueIdValue);
            }
        }

        // Week Id is optional, so just make sure it is a two digit number between 1 and 17.
        if (weekIdIndex != -1)
        {
            if (weekIdIndex == totalCommandLineLength)
            {
                Log.Error("Week ID was not provided.");
                throw new InvalidOptionException("A weekId must be provided.");
            }
            else
            {
                var weekIdValue = args[weekIdIndex + 1];
                weekIdValue = weekIdValue.Trim();
                var pattern = @"^(1[0-7]|[1-9])$";
                if (!Regex.IsMatch(weekIdValue, pattern))
                {
                    // CurrywareLogHandler.AddLog("Week ID has to be between 1 and 17", LogLevel.Error);
                    Log.Error("Week ID has to be between 1 and 17");
                    throw new InvalidOptionException("Week must be between 1 and 17.");
                }

                gameStatInformation.Week = Convert.ToInt32(weekIdValue);
            }
        }

        if (positionIndex == -1) return gameStatInformation;
        
        if (positionIndex == totalCommandLineLength)
        {
            // CurrywareLogHandler.AddLog("Position was not provided.", LogLevel.Error);
            Log.Error("Position was not provided.");
            throw new InvalidOptionException("A position must be provided.");
        }
        else
        {
            var positionValue = args[positionIndex + 1].ToLower().Trim();
            switch (positionValue)
            {
                case "qb":
                    gameStatInformation.PlayerPosition = "QB";
                    break;
                case "rb":
                    gameStatInformation.PlayerPosition = "RB";
                    break;
                case "wr":
                    gameStatInformation.PlayerPosition = "WR";
                    break;
                case "te":
                    gameStatInformation.PlayerPosition = "TE";
                    break;
                default:
                    gameStatInformation.PlayerPosition = "None";
                    break;
            }
        }
        return gameStatInformation;
    }
}