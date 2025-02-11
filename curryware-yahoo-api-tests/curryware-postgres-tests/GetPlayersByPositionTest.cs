using Xunit;

using curryware_postgres_library;

namespace curryware_yahoo_api_tests.curryware_postgres_tests;

public class PlayersTests
{
    [Fact]
    public async Task GetPlayersByPositionTest()
    {
        Environment.SetEnvironmentVariable("POSTGRES_USERNAME", "scot");
        Environment.SetEnvironmentVariable("POSTGRES_PASSWORD", "AirWatch1");
        Environment.SetEnvironmentVariable("POSTGRES_PORT", "5432");
        Environment.SetEnvironmentVariable("POSTGRES_DATABASE", "currywarefantasy");
        Environment.SetEnvironmentVariable("POSTGRES_SERVER", "localhost");
        string position = "QB";
        var playersByPosition = await PostgresLibrary.GetPlayerIdsByPosition(position);
        Assert.Contains("Aaron Rodgers", playersByPosition);
    }
}