// TODO: Remove this migration is done.
// using Xunit;
//
// using curryware_yahoo_parsing_library.TransactionApi;
//
// namespace curryware_yahoo_api_tests.curryware_parsing_library_tests;
//
// public class GetTransactionsTest
// {
//     [Fact]
//     public async Task GetLeagueTransactionsTest()
//     {
//         const int gameId = 449;
//         const int leagueKey = 483521;
//         const int startNumber = 0;
//
//         var playersApi = await GetLeagueTransactionApi.GetAllTransactions(gameId, leagueKey, startNumber);
//         Assert.True(playersApi?.Length > 0);
//     }
// }