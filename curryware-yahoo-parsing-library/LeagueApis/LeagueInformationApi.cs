using curryware_yahoo_parsing_library.FirebaseOAuthCallHandler;
using curryware_yahoo_parsing_library.HttpHandler;
using curryware_yahoo_parsing_library.XmlParsers.LeagueParsers;
using Serilog;

namespace curryware_yahoo_parsing_library.LeagueApis;

public class LeagueInformationApi
{
    public async Task<string?> GetLeagueInformation(int leagueId = 483521, int gameId = 449)
    {
        // This is the endpoint for the league information.
        var yahooLeagueKey = gameId.ToString() + ".l." + leagueId.ToString();
        var leagueInformationEndpoint = "https://fantasysports.yahooapis.com/fantasy/v2/league/" + yahooLeagueKey;
        // CurrywareLogHandler.AddLog("Calling League Information API: " + leagueInformationEndpoint, LogLevel.Debug);
        Log.Debug("LeagueInformation API - Calling League Information API: " + leagueInformationEndpoint + "");

        var oAuthToken = await FirebaseOAuthCall.GetOAuthTokenFromFirebase();
        if (oAuthToken.Substring(0, 6) == "Error:")
            return null;
        // CurrywareLogHandler.AddLog("Got OAuth Token from Firebase", LogLevel.Debug);
        Log.Debug("Got OAuth Token from Firebase");
        
        var leagueInformationXml = await HttpRequestHandler.MakeYahooApiCall(leagueInformationEndpoint, 
            oAuthToken);
        if (leagueInformationXml.Substring(0, 6) == "Error:")
            return null;
        // CurrywareLogHandler.AddLog("Got League Information XML", LogLevel.Debug);
        Log.Debug("Got League Information XML");
        
        var leagueInformationJson = LeagueInformationParser.GetLeagueInformationFromXml(leagueInformationXml);
        if (leagueInformationJson.Substring(0, 6) == "Error:")
            return null;
        // CurrywareLogHandler.AddLog("Got League Information JSON", LogLevel.Debug);
        Log.Debug("Got League Information JSON");
        
        return leagueInformationJson;
    }
}   