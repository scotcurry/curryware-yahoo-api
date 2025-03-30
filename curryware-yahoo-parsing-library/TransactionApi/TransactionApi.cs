using curryware_yahoo_parsing_library.HttpHandler;
using curryware_yahoo_parsing_library.FirebaseOAuthCallHandler;
using curryware_yahoo_parsing_library.XmlParsers.TransactionParser;
using Serilog;

namespace curryware_yahoo_parsing_library.TransactionApi;

public abstract class GetLeagueTransactionApi
{
    public static async Task<string?> GetAllTransactions(int gameId, int leagueId, int startNumber = 0)
    {
        var playerEndpoint = "https://fantasysports.yahooapis.com/fantasy/v2/league/{game_id}.l.{league_id}/transactions";
        playerEndpoint = playerEndpoint.Replace("{game_id}", gameId.ToString());
        playerEndpoint = playerEndpoint.Replace("{league_id}", leagueId.ToString());
        
        if (startNumber > 0)
        {
            playerEndpoint = $"{playerEndpoint}?start={startNumber}";
        }
        
        // CurrywareLogHandler.AddLog("Calling League Information API: " + playerEndpoint, LogLevel.Debug);
        Log.Debug("Calling League Information API: {playerEndpoint}", playerEndpoint);

        var oAuthToken = await FirebaseOAuthCall.GetOAuthTokenFromFirebase();
        if (oAuthToken[..6] == "Error:")
            return null;
        // CurrywareLogHandler.AddLog("Got OAuth Token from Firebase", LogLevel.Debug);
        Log.Debug(string.Concat("Got OAuth Token from Firebase: ", oAuthToken.AsSpan(0, 12)));

        var playerInformationXml = await HttpRequestHandler.MakeYahooApiCall(playerEndpoint,
            oAuthToken);
        if (playerInformationXml[..6] == "Error:")
            return null;
        // CurrywareLogHandler.AddLog("Got League Information XML", LogLevel.Debug);
        Log.Debug(string.Concat("Got Player Information XML: ", playerInformationXml.AsSpan(0, 40), ""));
        
        var allTransactionsJson = LeagueTransactionsParser.GetParseLeagueTransactionsXml(playerInformationXml);
        return allTransactionsJson[..6] == "Error:" ? null : "debug";
    }
}