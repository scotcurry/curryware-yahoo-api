using Xunit;

using curryware_fantasy_command_line_tool.StatsCommands;

namespace curryware_yahoo_api_tests.curryware_command_line_tests;

public class GetPlayersFromDatabaseTest
{
    [Fact]
    public async Task GetPlayersFromPostgresDatabaseTest()
    {
        Environment.SetEnvironmentVariable("POSTGRES_USERNAME", "scot");
        Environment.SetEnvironmentVariable("POSTGRES_PASSWORD", "AirWatch1");
        Environment.SetEnvironmentVariable("POSTGRES_PORT", "5432");
        Environment.SetEnvironmentVariable("POSTGRES_DATABASE", "currywarefantasy");
        Environment.SetEnvironmentVariable("POSTGRES_SERVER", "localhost");
        var playerPosition = "RB";
        
        var players = await GetPlayersFromPostgres.GetPlayersByPosition(playerPosition);
        Assert.True(players.Count > 0);
    }
}