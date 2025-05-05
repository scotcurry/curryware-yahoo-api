using Xunit.Abstractions;

using curryware_yahoo_parsing_library.LeagueApis;

namespace curryware_yahoo_parsing_library_tests;

public class LeagueStatSettingsTest(ITestOutputHelper output)
{
    [Fact]
    public async Task GetLeagueInformationTest()
    {
        output.WriteLine("Get League Information");
        var leagueInformationApi = new LeagueStatSettingsApi();
        var leagueJson = await LeagueStatSettingsApi.GetLeagueScoringInformation(461, 13407);
        var leagueKey = leagueJson?.Substring(18, 9);
        Assert.Equal(461134074, Convert.ToInt64(leagueKey));
    }
}