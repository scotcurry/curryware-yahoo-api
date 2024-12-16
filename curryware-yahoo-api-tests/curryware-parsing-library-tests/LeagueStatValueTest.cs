using curryware_yahoo_parsing_library.LeagueApis;
using Xunit;
using Xunit.Abstractions;

namespace curryware_yahoo_api_tests.curryware_parsing_library_tests;

public class LeagueStatValueTest(ITestOutputHelper output)
{
    [Fact]
    public async Task GetLeagueStatValueTest()
    {
        output.WriteLine("Get League Stat Value");
        var leagueStatsApi = new LeagueStatValueApi();
        var leagueStatsJson = await leagueStatsApi.GetLeagueStatValueInformation();
        var leagueKey = leagueStatsJson?.Substring(18, 10);
        Assert.Equal(4494835214, Convert.ToInt64(leagueKey));
    }
}