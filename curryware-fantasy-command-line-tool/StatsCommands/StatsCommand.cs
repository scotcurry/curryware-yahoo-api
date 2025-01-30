using curryware_kafka_command_line.CommandLineModels;
using curryware_log_handler;
using curryware_postgres_library;
using Microsoft.Extensions.Logging;

namespace curryware_kafka_command_line.StatsCommands;

internal abstract class StatsCommand
{
    internal static async Task<string> GetStats(GameStatsCommandLineParameters statsCommandLineParameters)
    {
        var gameId = statsCommandLineParameters.GameId;
        var playerPosition = statsCommandLineParameters.PlayerPosition;
        var statsWeek = statsCommandLineParameters.Week;
        var startNumber = 0;
        var totalBatches = 0;
        var morePlayers = true;
        var oauthToken = "NoToken";
        
        // This queries the Postgres database and pulls all the players at a given position.  This will be passed in
        // to get the stats.
        // Make sure to set environment variables for Postgres Server.
        var playerList = string.Empty;
        if (playerPosition != null)
            playerList = await PostgresLibrary.GetPlayerIdsByPosition(playerPosition);
        else
        {
            CurrywareLogHandler.AddLog("No player position specified.", LogLevel.Error);
            Environment.Exit(200);
        }
        
        // TODO: Fix this
        return "scot";
    }
}