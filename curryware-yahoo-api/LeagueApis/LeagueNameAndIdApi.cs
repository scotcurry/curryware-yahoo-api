using curryware_yahoo_api.HandlerClasses;
using curryware_yahoo_api.LeagueSettingsModel;
using curryware_yahoo_api.XMLParsers.LeagueParsers;

namespace curryware_yahoo_api.LeagueApis;

public class LeagueNameAndIdApi
{
    
    private readonly string _allPlayerEndpoint = "league/{game_number}.l.{team_number}/settings";
    
    public async Task<LeagueNameModel> LeagueInformation(int gameNumber, int teamNumber, string oauthToken)
    {
        string gameNumberString = gameNumber.ToString();
        string teamNumberString = teamNumber.ToString();
        
        string endpointToCall = _allPlayerEndpoint.Replace("{game_number}", gameNumberString);
        endpointToCall = endpointToCall.Replace("{team_number}", teamNumberString);
        
        var xmlToParse = await HttpRequestHandler.MakeYahooApiCall(endpointToCall, oauthToken);
        var leagueSettingsParser = new LeagueSettingsParser();
        var leagueName = leagueSettingsParser.GetLeagueName(xmlToParse);

        return leagueName;
    }
}