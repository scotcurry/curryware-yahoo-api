using Xunit;
using Xunit.Abstractions;

using curryware_yahoo_parsing_library.LeagueApis;

namespace curryware_yahoo_api_tests.curryware_parsing_library_tests;

public class LeagueStatSettingsTest(ITestOutputHelper output)
{
    [Fact]
    public async Task GetLeagueInformationTest()
    {
        output.WriteLine("Get League Information");
        var leagueInformationApi = new LeagueStatSettingsApi();
        var leagueJson = await LeagueStatSettingsApi.GetLeagueScoringInformation();
        var leagueKey = leagueJson?.Substring(18, 10);
        Assert.Equal(4494835214, Convert.ToInt64(leagueKey));
    }
}