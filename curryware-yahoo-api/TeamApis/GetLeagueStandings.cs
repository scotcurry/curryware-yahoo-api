using Serilog;
using Serilog.Formatting.Json;

using curryware_yahoo_api.FirebaseOAuthCallHandler;
using curryware_yahoo_api.HandlerClasses;
using curryware_yahoo_api.TeamModels;
using curryware_yahoo_api.XMLParsers.LeagueParsers;

namespace curryware_yahoo_api.TeamApis;

public class LeagueStandings
{
    private readonly string _leagueStandingsEndpoint = "league/{game_number}.l.{team_number}/standings";

    public async Task<List<LeagueStandingsTeamModel>> GetLeagueStandings(int gameNumber, int teamNumber)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console(new JsonFormatter())
            .CreateLogger();
        
        var oauthToken = await FirebaseOAuthCall.GetOAuthTokenFromFirebase();
        string gameNumberString = gameNumber.ToString();
        string teamNumberString = teamNumber.ToString();
        string endpointToCall = _leagueStandingsEndpoint.Replace("{game_number}", gameNumberString);
        endpointToCall = endpointToCall.Replace("{team_number}", teamNumberString);

        var outerXmlToParse = string.Empty;
        try
        {
            var xmlToParse = await HttpRequestHandler.MakeYahooApiCall(endpointToCall, oauthToken);
            outerXmlToParse = xmlToParse;
        }
        catch (HttpRequestException requestException)
        {
            Log.Error("Error calling {0}, Error Message: {1}", endpointToCall, requestException.Message);
        }

        var leagueStandingsParser = new LeagueStandingsParser();
        var parsedLeagueStandings = leagueStandingsParser.GetLeagueStandings(outerXmlToParse);
        return parsedLeagueStandings;
    }
}