using Xunit;

using curryware_postgres_library;

namespace curryware_yahoo_api_tests.curryware_postgres_tests;

public class PlayersTests
{
    [Fact]
    public async Task GetPlayersByPositionTest()
    {
        // TODO: Get this test working.
        string position = "QB";
        var playersByPosition = await PostgresLibrary.GetPlayerIdsByPosition(position);
        Assert.Equal(1, 2);
    }
}