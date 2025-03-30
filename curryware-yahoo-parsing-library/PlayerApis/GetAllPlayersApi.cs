using curryware_yahoo_parsing_library.HttpHandler;
using curryware_yahoo_parsing_library.FirebaseOAuthCallHandler;
using curryware_yahoo_parsing_library.XmlParsers.LeaguePlayers;
using Serilog;

namespace curryware_yahoo_parsing_library.PlayerApis;

public abstract class GetAllPlayersApi
{
    public static async Task<string?> GetAllPlayers(string oAuthToken, int gameId, int leagueId, int startNumber, string status = "None",
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
        
        if (playerEndpoint.Contains('?'))
            playerEndpoint += "&start=" + startNumber;
        else
            playerEndpoint += "?start=" + startNumber;
        
        // CurrywareLogHandler.AddLog("Calling League Information API: " + playerEndpoint, LogLevel.Debug);
        Log.Debug("Calling League Information API: " + playerEndpoint);

        if (oAuthToken == "NoToken")
        {
            oAuthToken = await FirebaseOAuthCall.GetOAuthTokenFromFirebase();
            if (oAuthToken[..6] == "Error:")
                return null;
            // CurrywareLogHandler.AddLog("Got OAuth Token from Firebase", LogLevel.Debug);
            Log.Debug(string.Concat("Got OAuth Token from Firebase: ", oAuthToken.AsSpan(0, 12)));
        }

        var playerInformationXml = await HttpRequestHandler.MakeYahooApiCall(playerEndpoint,
            oAuthToken);
        if (playerInformationXml[..6] == "Error:")
            return null;
        // CurrywareLogHandler.AddLog("Got League Information XML", LogLevel.Debug);
        Log.Debug(string.Concat("Got Player Information XML: ", playerInformationXml.AsSpan(0, 40), ""));

        var allPlayersJson = LeaguePlayerParser.GetParseLeaguePlayerXml(playerInformationXml, oAuthToken);
        if (allPlayersJson[..6] == "Error:")
            return null;

        // CurrywareLogHandler.AddLog("Got League Information JSON", LogLevel.Debug);
        Log.Debug(string.Concat("Got Player Information JSON: ", allPlayersJson.AsSpan(0, 40), ""));
        return allPlayersJson;
    }
}