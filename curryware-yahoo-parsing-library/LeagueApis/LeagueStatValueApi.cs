using curryware_yahoo_parsing_library.FirebaseOAuthCallHandler;
using curryware_yahoo_parsing_library.HttpHandler;
using curryware_yahoo_parsing_library.LogHandler;
using curryware_yahoo_parsing_library.XmlParsers.LeagueParsers;
using Microsoft.Extensions.Logging;

namespace curryware_yahoo_parsing_library.LeagueApis;

public abstract class LeagueStatValueApi
{
    public static async Task<string?> GetLeagueStatValueInformation(int gameId = 449, int leagueId = 483521)
    {
        // This is the endpoint for the league information.
        var leagueSettingEndpoint =
            "https://fantasysports.yahooapis.com/fantasy/v2/league/{game_id}.l.{league_id}/settings";
        leagueSettingEndpoint = leagueSettingEndpoint.Replace("{game_id}", gameId.ToString());
        leagueSettingEndpoint = leagueSettingEndpoint.Replace("{league_id}", leagueId.ToString());
        CurrywareLogHandler.AddLog("Calling League Information API: " + leagueSettingEndpoint, LogLevel.Debug);

        var oAuthToken = await FirebaseOAuthCall.GetOAuthTokenFromFirebase();
        if (oAuthToken.Substring(0, 6) == "Error:")
            return null;
        CurrywareLogHandler.AddLog("Got OAuth Token from Firebase", LogLevel.Debug);

        var leagueInformationXml = await HttpRequestHandler.MakeYahooApiCall(leagueSettingEndpoint,
            oAuthToken);
        if (leagueInformationXml.Substring(0, 6) == "Error:")
            return null;
        CurrywareLogHandler.AddLog("Got League Information XML", LogLevel.Debug);
        
        var leagueStatSValuesJson = LeagueStatValueParser.GetLeagueStatValuesFromXml(leagueInformationXml, gameId, leagueId);
        if (leagueStatSValuesJson.Substring(0, 6) == "Error:")
            return null;
        CurrywareLogHandler.AddLog("Got League Stat Settings JSON", LogLevel.Debug);
        
        return leagueStatSValuesJson;
    }
}