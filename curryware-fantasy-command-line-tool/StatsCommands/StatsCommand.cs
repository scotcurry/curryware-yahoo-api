using curryware_fantasy_command_line_tool.CommandLineModels;

using Serilog;

namespace curryware_fantasy_command_line_tool.StatsCommands;

public abstract class StatsCommand
{
    public static async Task<List<string>> GetStats(GameStatsCommandLineParameters statsCommandLineParameters)
    {
        var gameId = statsCommandLineParameters.GameId;
        var playerPosition = statsCommandLineParameters.PlayerPosition;
        var statsWeek = statsCommandLineParameters.Week;

        // Pull all the players by position from the Postgres database.
        List<string> allPlayersJson;
        if (playerPosition != null)
        {
            allPlayersJson = await GetPlayersFromPostgres.GetPlayersByPosition(playerPosition);
        }
        else
        {
            return [];
        }

        // Get all of the statistics for the pulled players.
        var playerStatsJson = await GetPlayerStats.GetPlayerStatBatch(allPlayersJson, gameId, statsWeek);
        if (playerStatsJson.Count > 0)
        {
            foreach (var currentBatch in playerStatsJson)
            {
                var success = await AddStatsToKafkaQueue.AddStatsToQueue(currentBatch);
                if (!success)
                {
                    // CurrywareLogHandler.AddLog("Failed to add stats to Kafka queue.", LogLevel.Error);
                    Log.Error("Failed to add stats to Kafka queue.");
                }
            }
        }
        
        return playerStatsJson;
    }
}