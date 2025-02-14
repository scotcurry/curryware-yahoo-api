using curryware_yahoo_parsing_library.StatsApis;
using curryware_yahoo_parsing_library.FirebaseOAuthCallHandler;

namespace curryware_fantasy_command_line_tool.StatsCommands;

public abstract class GetPlayerStats
{
    public static async Task<List<string>> GetPlayerStatBatch(List<string> playerIds, int gameId, int weekId)
    {
        var allBatches = new List<string>();
        const int playersInBatch = 25;
        var oAuthToken = await FirebaseOAuthCall.GetOAuthTokenFromFirebase();
        var totalPlayers = playerIds.Count;
        
        for (var counter = 0; counter < playerIds.Count; counter += playersInBatch)
        {
            var playersRemaining = totalPlayers - counter;
            var batchSize = Math.Min(playersInBatch, playersRemaining);
            var batch = playerIds.GetRange(counter, batchSize).ToList();
            var statsBatch = await GetWeeklyStatsApi.GetWeeklyStats(oAuthToken, gameId, weekId, batch);
            allBatches.Add(statsBatch);
        }

        return allBatches;
    }
}