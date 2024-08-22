using curryware_yahoo_api.HandlerClasses;
using curryware_yahoo_api.XMLParsers.LeaguePlayers;

namespace curryware_yahoo_api.PlayerApis;

public class GetAllPlayersApi
{
    private int _playerCount;
    private int _pageNumber;
    private bool _havePlayersToRetrieve = true;
    private readonly string _allPlayerEndpoint = "league/{game_number}.l.{team_number}/players";

    public async Task<int> GetAllPlayers(int gameNumber, int teamNumber, string oauthToken)
    {
        string gameNumberString = gameNumber.ToString();
        string teamNumberString = teamNumber.ToString();
        string endpointToCall = _allPlayerEndpoint.Replace("{game_number}", gameNumberString);
        endpointToCall = endpointToCall.Replace("{team_number}", teamNumberString);
        
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
                Console.WriteLine("Player Count: " + _playerCount);
            }
        }

        return _playerCount;
    }
}