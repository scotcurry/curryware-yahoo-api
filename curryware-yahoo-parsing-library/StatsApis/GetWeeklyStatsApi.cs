using curryware_yahoo_parsing_library.FirebaseOAuthCallHandler;
using curryware_yahoo_parsing_library.HttpHandler;
using curryware_yahoo_parsing_library.LogHandler;
using curryware_yahoo_parsing_library.XmlParsers.StatsParsers;
using Microsoft.Extensions.Logging;

namespace curryware_yahoo_parsing_library.StatsApis;

public abstract class GetWeeklyStatsApi
{
    public static async Task<string> GetWeeklyStats(string oAuthToken, int gameId, int week, List<string> playerKeys)
    {
        var statsJson = string.Empty;
        var endpoint = "https://fantasysports.yahooapis.com/fantasy/v2/players;player_keys={playerKeysToken}/stats" +
                       "?type=week&week={week}";
        endpoint = endpoint.Replace("{week}", Convert.ToString(week));
        var playerBatchStrings = BuildPlayerBatchStrings(gameId, playerKeys);
        foreach (var playersString in playerBatchStrings)
        {
            endpoint = endpoint.Replace("{playerKeysToken}", playersString);
            CurrywareLogHandler.AddLog($"Getting stats for endpoint: {endpoint}", LogLevel.Debug);

            if (oAuthToken == "NoToken")
            {
                oAuthToken = await FirebaseOAuthCall.GetOAuthTokenFromFirebase();
                if (oAuthToken.Substring(0, 6) == "Error:")
                {
                    CurrywareLogHandler.AddLog("Error getting OAuth Token from Firebase GetWeeklyStatsAPI",
                        LogLevel.Error);
                    return "Error";
                }
                CurrywareLogHandler.AddLog("Got OAuth Token from Firebase", LogLevel.Debug);
            }

            var statInformationXml = await HttpRequestHandler.MakeYahooApiCall(endpoint,
                    oAuthToken);
            if (statInformationXml.Substring(0, 6) == "Error:")
            {
                CurrywareLogHandler.AddLog("Error getting stats from Yahoo API", LogLevel.Error);
                return "Error";
            }
            CurrywareLogHandler.AddLog("Got stats XML", LogLevel.Debug);
            
            statsJson = WeeklyStatParser.WeeklyStats(statInformationXml, oAuthToken, gameId);
            if (statsJson.Substring(0, 6) == "Error:")
            {
                CurrywareLogHandler.AddLog("Error getting stats from Yahoo API", LogLevel.Error);
            }
        }
        return statsJson;
    }

    private static List<string> BuildPlayerBatchStrings(int gameId, List<string> playerKeys)
    {
        var playerBatchStrings = new List<string>();
        var batchSize = 10;
        var totalBatches = playerKeys.Count / batchSize;
        var lastBatch = playerKeys.Count % batchSize;
        
        // This builds a string of up to 10 players which is the batch size.
        for (var currentBatch = 0; currentBatch < totalBatches; currentBatch++)
        {
            var batchOfPlayers = playerKeys.Skip(currentBatch).Take(batchSize).ToList();
            var playerBatchString = string.Empty;
            // This builds out the individual player lists and then appends them to the total list.
            for (var currentPlayer = 0; currentPlayer < batchOfPlayers.Count; currentPlayer++)
            {
                if (currentPlayer == batchOfPlayers.Count - 1)
                {
                    playerBatchString += Convert.ToString(gameId) + ".p." + playerKeys[currentPlayer];
                }
                else
                {
                    playerBatchString += Convert.ToString(gameId) + ".p." + playerKeys[currentPlayer] + ",";
                }
            }
            playerBatchStrings.Add(playerBatchString);
        }

        if (lastBatch <= 0) return playerBatchStrings;
        {
            var lastPlayers = playerKeys.Skip(totalBatches * batchSize).Take(lastBatch).ToList();
            var lastPlayersBatchString = string.Empty;
            for (var currentPlayer = 0; currentPlayer < lastPlayers.Count; currentPlayer++)
            {
                if (currentPlayer == lastPlayers.Count - 1)
                {
                    lastPlayersBatchString += Convert.ToString(gameId) + ".p." + lastPlayers[currentPlayer];
                }
                else
                {
                    lastPlayersBatchString += Convert.ToString(gameId) + ".p." + lastPlayers[currentPlayer] + ",";
                }
            }
            playerBatchStrings.Add(lastPlayersBatchString);
        }
        return playerBatchStrings;
    }
}