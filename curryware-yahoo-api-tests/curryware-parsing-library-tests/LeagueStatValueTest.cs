// TODO: Remove this after migration.
// using curryware_yahoo_parsing_library.LeagueApis;
// using Xunit;
// using Xunit.Abstractions;
//
// namespace curryware_yahoo_api_tests.curryware_parsing_library_tests;
//
// public class LeagueStatValueTest(ITestOutputHelper output)
// {
//     [Fact]
//     public async Task GetLeagueStatValueTest()
//     {
//         output.WriteLine("Get League Stat Value");
//         var gameId = 449;
//         var leagueKey = 483521;
//         var leagueStatsJson = await LeagueStatValueApi.GetLeagueStatValueInformation(gameId, leagueKey);
//         Assert.True(leagueStatsJson?.Length > 0);
//     }
// }