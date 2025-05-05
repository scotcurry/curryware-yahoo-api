using curryware_yahoo_parsing_library.FirebaseOAuthCallHandler;
using curryware_yahoo_parsing_library.LeagueApis;

namespace curryware_fantasy_command_line_tool.StatsCommands;

public abstract class GetLeagueStats
{
    public static async Task<string> GetStatInfoJson(int gameId, int leagueId)
    {
        var oAuthToken = await FirebaseOAuthCall.GetOAuthTokenFromFirebase();
        var leagueSettings = await LeagueStatSettingsApi.GetLeagueScoringInformation(leagueId, gameId);
        return leagueSettings ?? "Error";
    }
}