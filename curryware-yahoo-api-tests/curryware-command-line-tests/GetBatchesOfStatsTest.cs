// TODO: Remove this. Don't do it until after a commit to make sure tests are working correctly.
// using Xunit;
// using curryware_fantasy_command_line_tool.StatsCommands;
// using Xunit.Abstractions;
//
// namespace curryware_yahoo_api_tests.curryware_command_line_tests;
//
// public class GetBatchesOfStatsTest
// {
//     private readonly ITestOutputHelper output;
//
//     public GetBatchesOfStatsTest(ITestOutputHelper output)
//     {
//         this.output = output;
//     }
//     
//     [Fact]
//     public async Task GetBatchesTest()
//     {
//         Environment.SetEnvironmentVariable("POSTGRES_USERNAME", "scot");
//         Environment.SetEnvironmentVariable("POSTGRES_PASSWORD", "AirWatch1");
//         Environment.SetEnvironmentVariable("POSTGRES_PORT", "5432");
//         Environment.SetEnvironmentVariable("POSTGRES_DATABASE", "currywarefantasy");
//         Environment.SetEnvironmentVariable("POSTGRES_SERVER", "localhost");
//         var playerPosition = "RB";
//         var gameId = 449;
//         var weekId = 2;
//         
//         var players = await GetPlayersFromPostgres.GetPlayersByPosition(playerPosition);
//         var playerBatch = GetPlayerStats.GetPlayerStatBatch(players, gameId, weekId);
//         Assert.Equal(2, weekId);
//     }
// }