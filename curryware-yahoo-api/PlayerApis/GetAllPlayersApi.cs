using System.Text.Json;
using Serilog;
using Serilog.Formatting.Json;

using curryware_yahoo_api.HandlerClasses;
using curryware_yahoo_api.XMLParsers.LeaguePlayers;
using curryware_yahoo_api.KafkaHandlers;

namespace curryware_yahoo_api.PlayerApis;

//  This calls the Yahoo API to get all the players in a league, then parses the XML to get the player information.
// Serializes the list returned to a JSON file to send to Kafka.
public class GetAllPlayersApi
{
    private int _playerCount;
    private int _pageNumber;
    private bool _havePlayersToRetrieve = true;
    private readonly string _allPlayerEndpoint = "league/{game_number}.l.{team_number}/players";

    public async Task<int> GetAllPlayers(int gameNumber, int teamNumber, string oauthToken)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console(new JsonFormatter())
            .CreateLogger();
        
        string gameNumberString = gameNumber.ToString();
        string teamNumberString = teamNumber.ToString();
        string endpointToCall = _allPlayerEndpoint.Replace("{game_number}", gameNumberString);
        endpointToCall = endpointToCall.Replace("{team_number}", teamNumberString);
        
        var jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = false
        };
        
        while (_havePlayersToRetrieve)
        {
            var pageToRetrieve = endpointToCall + "?start=" + _pageNumber;
            var xmlToParse = await HttpRequestHandler.MakeYahooApiCall(pageToRetrieve, oauthToken);
            var playerList = LeaguePlayerParser.GetParseLeaguePlayerXml(xmlToParse);

            if (playerList.Count == 0)
            {
                _havePlayersToRetrieve = false;
            }
            else
            {
                _pageNumber += 25;
                _playerCount += playerList.Count;
                var jsonString = JsonSerializer.Serialize(playerList, jsonOptions);
                var success = await PlayerProducer.SendPlayerData("PlayerTopic", jsonString);
                if (!success)
                {
                    Log.Error("Error sending player data to Kafka");
                }
                Console.WriteLine("Player Count: " + _playerCount);
            }
        }

        return _playerCount;
    }
}