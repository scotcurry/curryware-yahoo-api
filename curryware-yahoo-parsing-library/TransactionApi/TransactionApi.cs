using curryware_yahoo_parsing_library.HttpHandler;
using curryware_yahoo_parsing_library.LogHandler;
using curryware_yahoo_parsing_library.FirebaseOAuthCallHandler;
using curryware_yahoo_parsing_library.XmlParsers.TransactionParser;
using Microsoft.Extensions.Logging;

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
        
        CurrywareLogHandler.AddLog("Calling League Information API: " + playerEndpoint, LogLevel.Debug);

        var oAuthToken = await FirebaseOAuthCall.GetOAuthTokenFromFirebase();
        if (oAuthToken.Substring(0, 6) == "Error:")
            return null;
        CurrywareLogHandler.AddLog("Got OAuth Token from Firebase", LogLevel.Debug);

        var playerInformationXml = await HttpRequestHandler.MakeYahooApiCall(playerEndpoint,
            oAuthToken);
        if (playerInformationXml.Substring(0, 6) == "Error:")
            return null;
        CurrywareLogHandler.AddLog("Got League Information XML", LogLevel.Debug);
        
        var allTransactionsJson = LeagueTransactionsParser.GetParseLeagueTransactionsXml(playerInformationXml);
        if (allTransactionsJson.Substring(0, 6) == "Error:")
            return null;

        return "debug";
    }
}