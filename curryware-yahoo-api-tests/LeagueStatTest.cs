using Xunit;
using curryware_yahoo_api.HandlerClasses;
using curryware_yahoo_api.FirebaseOAuthCallHandler;
using curryware_yahoo_api.XMLParsers;

namespace curryware_yahoo_api_tests;

public class LeagueStatTest
{
    [Fact]
    public async Task GetLeagueStatsTest()
    {
        var xmlUrl = "league/423.l.661655/settings";
        var oauthToken = await FirebaseOAuthCall.GetOAuthTokenFromFirebase();
        var statsXml = await HttpRequestHandler.MakeYahooApiCall(xmlUrl, oauthToken);
        
        var leagueStatsParser = new LeagueStatParser();
        var leagueStats = leagueStatsParser.GetLeagueStatNamesAndValues(statsXml);
        Assert.Equal(36, leagueStats.Count);
    }
}