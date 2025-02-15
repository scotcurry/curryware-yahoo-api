// TODO: Remove after migration
// using curryware_yahoo_api.TeamApis;
// using Xunit;
//
// namespace curryware_yahoo_api_tests;
//
// public class LeagueStandingsTest
// {
//     [Fact]
//     public async Task GetLeagueStandings()
//     {
//         var gameNumber = 449;
//         var leagueNumber = 483521;
//
//         var leagueSettings = new LeagueStandings();
//         var allLeagueTeams = await leagueSettings.GetLeagueStandings(gameNumber, leagueNumber);
//         
//         var numberOfTeams = allLeagueTeams.Count;
//         var correctTeamManagerCombo = false;
//         foreach (var currentTeam in allLeagueTeams)
//         {
//             if (currentTeam is { ManagerNickName: "Scot Curry", TeamKey: "449.l.483521.t.10" })
//                 correctTeamManagerCombo = true;
//         }
//
//         var assertion = (correctTeamManagerCombo && numberOfTeams == 12);
//         Assert.True(assertion);
//     }
// }