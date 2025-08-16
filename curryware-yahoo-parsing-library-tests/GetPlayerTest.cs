using curryware_yahoo_parsing_library.PlayerApis;

namespace curryware_yahoo_parsing_library_tests;

public class GetPlayersTest
{
    [Fact]
    public async Task GetTwentyFivePlayersTest()
    {
        const int gameId = 461;
        const int leagueKey = 394957;
        const int startNumber = 70;
        const string oAuthToken = "NoToken";

        var playersApi = await GetAllPlayersApi.GetAllPlayers(oAuthToken, gameId, leagueKey, startNumber);
        Assert.True(playersApi?.Length > 0);
    }
}