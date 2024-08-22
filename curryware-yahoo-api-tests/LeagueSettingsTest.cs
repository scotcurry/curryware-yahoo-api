using Xunit;

using curryware_yahoo_api.HandlerClasses;
using curryware_yahoo_api.FirebaseOAuthCallHandler;
using curryware_yahoo_api.XMLParsers;
using curryware_yahoo_api.LeagueApis;

namespace curryware_yahoo_api_tests;

public class LeagueSettingsTest
{
    [Fact]
    public async Task GetLeagueInfoParserTest()
    {
        var xmlUrl = "league/423.l.661655/settings";
        var oauthToken = await FirebaseOAuthCall.GetOAuthTokenFromFirebase();
        var playersXml = await HttpRequestHandler.MakeYahooApiCall(xmlUrl, oauthToken);

        var leagueParserCall = new LeagueSettingsParser();
        var leagueNameInfo = leagueParserCall.GetLeagueName(playersXml);
        Assert.Equal("Datadog FSE West Central", leagueNameInfo.LeagueName);
    }

    [Fact]
    public async Task GetLeagueInfoTest()
    {
        var gameNumber = 423;
        var teamNumber = 661655;
        var oauthToken = await FirebaseOAuthCall.GetOAuthTokenFromFirebase();

        var leagueSettingsParser = new LeagueNameAndIdApi();
        var leagueInfo = await leagueSettingsParser.LeagueInformation(gameNumber, teamNumber, oauthToken);
        Assert.Equal("423.l.661655", leagueInfo.LeagueKey);
    }
}