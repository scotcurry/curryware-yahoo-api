using curryware_yahoo_parsing_library.LeagueApis;

namespace curryware_yahoo_parsing_library_tests;

public class LeagueInformationTest
{
    [Fact]
    public static async Task GetLeagueInformationTest()
    {
        var leagueInformationApi = new LeagueInformationApi();
        var leagueJson = await leagueInformationApi.GetLeagueInformation();
        var leagueKey = leagueJson?.Substring(14, 12);
        Assert.Equal("449.l.483521", leagueKey);
    }
}