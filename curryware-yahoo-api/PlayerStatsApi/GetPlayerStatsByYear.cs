using curryware_yahoo_api.HandlerClasses;
using curryware_yahoo_api.PlayerStatModels;

namespace curryware_yahoo_api.PlayerStatsApi;

public class GetPlayerStatsByYear
{
    private int _pageNumber;
    private bool _havePlayersToRetrieve = true;
    private readonly string _allPlayerEndpoint = "league/{game_number}.l.{team_number}/players/stats";

    public GetPlayerStatsByYear(int pageNumber)
    {
        _pageNumber = pageNumber;
    }

    public async Task<PlayerStatModel> GetPlayerStats(int gameNumber, int teamNumber, string oauthToken)
    {
        string gameNumberString = gameNumber.ToString();
        string teamNumberString = teamNumber.ToString();
        string endpointToCall = _allPlayerEndpoint.Replace("{game_number}", gameNumberString);
        endpointToCall = endpointToCall.Replace("{team_number}", teamNumberString);

        var playerStats = new PlayerStatModel();

        while (_havePlayersToRetrieve)
        {
            var pageToRetrieve = endpointToCall + "?start=" + _pageNumber;
            var xmlToParse = await HttpRequestHandler.MakeYahooApiCall(pageToRetrieve, oauthToken);
        }

        return playerStats;
    }
}