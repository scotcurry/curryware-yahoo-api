using Xunit;

using curryware_yahoo_parsing_library.StatsApis;

namespace curryware_yahoo_api_tests.curryware_parsing_library_tests;

public class StatsTests
{
    [Fact]
    public async Task GetWeeklyStatsTest()
    {
        const int gameId = 449;
        const int weekNumber = 1;
        var playerList = new List<string>();
        var oAuthToken = "NoToken";
        playerList.AddRange(["28638", "28839", "29235", "29369", "30115"]);
        playerList.AddRange(["30125", "30217", "30973", "30971", "31002"]);
        playerList.AddRange(["31833", "32671"]);
        
        var playerBatchString = await GetWeeklyStatsApi.GetWeeklyStats(oAuthToken, gameId, weekNumber, playerList);
        Assert.True(playerBatchString?.Length > 0);
    }
}