// using System.Text.Json;
// using Confluent.Kafka;
//
// using curryware_yahoo_api.HandlerClasses;
// using curryware_yahoo_api.XMLParsers.LeaguePlayers;
// using curryware_yahoo_api.KafkaHandlers;
// using curryware_yahoo_api.LogHandler;
//
// namespace curryware_yahoo_api.PlayerApis;
//
// //  This calls the Yahoo API to get all the players in a league, then parses the XML to get the player information.
// // Serializes the list returned to a JSON file to send to Kafka.
// public class GetAllPlayersApi
// {
//     private int _playerCount;
//     private int _pageNumber;
//     private bool _havePlayersToRetrieve = true;
//     private readonly string _allPlayerEndpoint = "league/{game_number}.l.{team_number}/players";
//
//     public async Task<int> GetAllPlayers(int gameNumber, int teamNumber, string oauthToken)
//     {
//         const string kafkaTopic = "PlayerTopic2";
//         string gameNumberString = gameNumber.ToString();
//         string teamNumberString = teamNumber.ToString();
//         string endpointToCall = _allPlayerEndpoint.Replace("{game_number}", gameNumberString);
//         endpointToCall = endpointToCall.Replace("{team_number}", teamNumberString);
//         
//         var jsonOptions = new JsonSerializerOptions
//         {
//             WriteIndented = false
//         };
//         
//         while (_havePlayersToRetrieve)
//         {
//             var pageToRetrieve = endpointToCall + "?start=" + _pageNumber;
//             var xmlToParse = await HttpRequestHandler.MakeYahooApiCall(pageToRetrieve, oauthToken);
//             var playerList = LeaguePlayerParser.GetParseLeaguePlayerXml(xmlToParse);
//
//             if (playerList.Count == 0)
//             {
//                 _havePlayersToRetrieve = false;
//             }
//             else
//             {
//                 _pageNumber += 25;
//                 _playerCount += playerList.Count;
//                 var jsonString = JsonSerializer.Serialize(playerList, jsonOptions);
//                 try
//                 {
//                     if (ValidateKafkaSettings.ValidateSettings())
//                     {
//                         var success = await PlayerProducer.SendPlayerData(kafkaTopic, jsonString);
//                         if (!success)
//                         {
//                             CurrywareLogHandler.AddLog("Error sending player data to Kafka", LogLevel.Error);
//                         }
//                     }
//                     CurrywareLogHandler.AddLog("Player Count: " + _playerCount, LogLevel.Information);
//                 }
//                 catch (KafkaException kafkaException)
//                 {
//                     CurrywareLogHandler.AddLog("Error processing player data: " + kafkaException.Message, LogLevel.Error);
//                 }
//             }
//         }
//         
//         CurrywareLogHandler.AddLog("All player count: " + _playerCount, LogLevel.Debug);
//         return _playerCount;
//     }
// }