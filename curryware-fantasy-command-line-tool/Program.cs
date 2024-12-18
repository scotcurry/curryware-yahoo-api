using System.Text.Json;
using Microsoft.Extensions.Logging;

using curryware_kafka_command_line.CommandLineHandlers;
using curryware_kafka_command_line.CommandLineModels;
using curryware_yahoo_parsing_library.PlayerApis;
using curryware_log_handler;
using curryware_yahoo_parsing_library.PlayerModels;
using curryware_kafka_producer_library;

namespace curryware_kafka_command_line;

internal abstract class Program
{
    public static async Task Main(string[] args)
    {
        try
        {
            var totalBatches = 0;
            var parsedCommandLineObject = CommandLineParser.ParseCommandLine(args);
            if (parsedCommandLineObject is PlayerCommandLineParameters parsedCommandLine)
                totalBatches = await RunGetPlayersCommand(parsedCommandLine);
            CurrywareLogHandler.AddLog($"Wrote {totalBatches} to Kafka queue", LogLevel.Debug);
        }
        catch (InvalidParameterException invalidParameterException)
        {
            CurrywareLogHandler.AddLog(invalidParameterException.Message, LogLevel.Error);
            PrintHelp();
            Environment.Exit(120);
        }
        catch (InvalidOperationException invalidOperationException)
        {
            CurrywareLogHandler.AddLog(invalidOperationException.Message, LogLevel.Error);
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
        Console.WriteLine("\t-l <leagueID>");
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

    // Because there are only 25 players to a page, this method gets a JSON string with all the players and the
    // number of players on the page, it then reserialize it with just the players to add it to the Kafka queue.
    private static async Task<int> RunGetPlayersCommand(PlayerCommandLineParameters playerCommandLineParameters)
    {
        var gameId = playerCommandLineParameters.GameId;
        var leagueId = playerCommandLineParameters.LeagueId;
        var playerPosition = playerCommandLineParameters.PlayerPosition;
        var playerStatus = playerCommandLineParameters.PlayerStatus;
        var startNumber = 0;
        var totalBatches = 0;
        var morePlayers = true;

        while (morePlayers)
        {
            var playerJson = string.Empty;
            if (playerPosition != "None" && playerStatus != "None")
                playerJson = await GetAllPlayersApi.GetAllPlayers(gameId, leagueId, startNumber,
                    status: playerPosition!, position: playerStatus!);
            if (playerPosition != "None" && playerStatus == "None")
                playerJson =
                    await GetAllPlayersApi.GetAllPlayers(gameId, leagueId, startNumber, status: playerPosition!);
            if (playerPosition == "None" && playerStatus != "None")
                playerJson = await GetAllPlayersApi.GetAllPlayers(gameId, leagueId, startNumber, status: "None",
                    position: playerStatus!);
            if (playerPosition == "None" && playerStatus == "None")
                playerJson = await GetAllPlayersApi.GetAllPlayers(gameId, leagueId, startNumber);

            if (playerJson == null) continue;
            var playersModel = JsonSerializer.Deserialize<PlayersListWithCount>(playerJson);
            if (playersModel == null) continue;
            if (playersModel.Players?.Count < 25)
                morePlayers = false;

            var justPlayers = JsonSerializer.Serialize(playersModel.Players);
            // TODO:  Start here.  Need to put this in a try catch along with everything else in this method.
            var kafkaResult = await KafkaProducer.CreateKafkaMessage("PlayerTopic", justPlayers);
            if (kafkaResult)
                totalBatches++;
        }
        
        return totalBatches;
    }
}
