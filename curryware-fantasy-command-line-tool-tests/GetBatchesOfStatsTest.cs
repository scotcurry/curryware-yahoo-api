using curryware_fantasy_command_line_tool.StatsCommands;

namespace curryware_fantasy_command_line_tool_tests;

public class GetBatchesOfStatsTest
{
    [Fact]
    public async Task GetBatchesTest()
    {
        Environment.SetEnvironmentVariable("POSTGRES_USERNAME", "scot");
        Environment.SetEnvironmentVariable("POSTGRES_PASSWORD", "AirWatch1");
        Environment.SetEnvironmentVariable("POSTGRES_PORT", "5432");
        Environment.SetEnvironmentVariable("POSTGRES_DATABASE", "currywarefantasy");
        Environment.SetEnvironmentVariable("POSTGRES_SERVER", "localhost");
        var playerPosition = "RB";
        var gameId = 449;
        var weekId = 2;
        
        var players = await GetPlayersFromPostgres.GetPlayersByPosition(playerPosition);
        var playerBatch = await GetPlayerStats.GetPlayerStatBatch(players, gameId, weekId);
        Assert.True(playerBatch.Count > 0);
    }
}