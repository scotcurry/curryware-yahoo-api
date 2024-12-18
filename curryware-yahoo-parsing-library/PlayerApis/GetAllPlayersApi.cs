using curryware_yahoo_parsing_library.HttpHandler;
using curryware_yahoo_parsing_library.LogHandler;
using curryware_yahoo_parsing_library.FirebaseOAuthCallHandler;
using curryware_yahoo_parsing_library.XmlParsers.LeaguePlayers;
using Microsoft.Extensions.Logging;

namespace curryware_yahoo_parsing_library.PlayerApis;

public abstract class GetAllPlayersApi
{
    public static async Task<string?> GetAllPlayers(int gameId, int leagueId, int startNumber, string status = "None",
        string position = "None")
    {
        // This is the endpoint for the league information.
        var playerEndpoint = "https://fantasysports.yahooapis.com/fantasy/v2/league/{game_id}.l.{league_id}/players";
        playerEndpoint = playerEndpoint.Replace("{game_id}", gameId.ToString());
        playerEndpoint = playerEndpoint.Replace("{league_id}", leagueId.ToString());
        if (status != "None" && position != "None")
            playerEndpoint += "?status=" + status + "&position=" + position;
        else if (status != "None")
            playerEndpoint += "?status=" + status;
        else if (position != "None")
            playerEndpoint += "?position=" + position;
        
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

        var allPlayersJson = LeaguePlayerParser.GetParseLeaguePlayerXml(playerInformationXml);
        if (allPlayersJson.Substring(0, 6) == "Error:")
            return null;

        CurrywareLogHandler.AddLog("Got League Information JSON", LogLevel.Debug);
        return allPlayersJson;
    }
}