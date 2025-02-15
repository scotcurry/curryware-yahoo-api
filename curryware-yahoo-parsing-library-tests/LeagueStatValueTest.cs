using Xunit.Abstractions;

using curryware_yahoo_parsing_library.LeagueApis;

namespace curryware_yahoo_parsing_library_tests;

public class LeagueStatValueTest(ITestOutputHelper output)
{
    [Fact]
    public async Task GetLeagueStatValueTest()
    {
        output.WriteLine("Get League Stat Value");
        var gameId = 449;
        const int leagueKey = 483521;
        var leagueStatsJson = await LeagueStatValueApi.GetLeagueStatValueInformation(gameId, leagueKey);
        Assert.True(leagueStatsJson?.Length > 0);
    }
}