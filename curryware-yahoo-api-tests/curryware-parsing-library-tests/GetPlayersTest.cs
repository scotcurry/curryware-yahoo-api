using Xunit;

using curryware_yahoo_parsing_library.PlayerApis;

namespace curryware_yahoo_api_tests.curryware_parsing_library_tests;

public class GetPlayersTest
{
    [Fact]
    public async Task GetTwentyFivePlayersTest()
    {
        const int gameId = 449;
        const int leagueKey = 483521;
        const int startNumber = 0;

        var playersApi = await GetAllPlayersApi.GetAllPlayers(gameId, leagueKey, startNumber);
        Assert.True(playersApi?.Length > 0);
    }
}