using curryware_postgres_library;

namespace curryware_postgres_library_tests;

public class PlayerTests
{
    [Fact]
    public async Task GetPlayersByPositionTest()
    {
        Environment.SetEnvironmentVariable("POSTGRES_USERNAME", "scot");
        Environment.SetEnvironmentVariable("POSTGRES_PASSWORD", "AirWatch1");
        Environment.SetEnvironmentVariable("POSTGRES_PORT", "5432");
        Environment.SetEnvironmentVariable("POSTGRES_DATABASE", "currywarefantasy");
        Environment.SetEnvironmentVariable("POSTGRES_SERVER", "localhost");
        const string position = "QB";
        var playersByPosition = await PostgresLibrary.GetPlayerIdsByPosition(position);
        Assert.Contains("Aaron Rodgers", playersByPosition);
    }
}