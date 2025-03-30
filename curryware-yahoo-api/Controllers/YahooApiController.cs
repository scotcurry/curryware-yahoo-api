// using curryware_yahoo_api.FirebaseOAuthCallHandler;
// using curryware_yahoo_api.JsonHandlers;
// using curryware_yahoo_api.LogHandler;
// using curryware_yahoo_api.PlayerApis;

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
//using curryware_yahoo_api.TeamApis;
using curryware_fantasy_command_line_tool.CommandLineHandlers;
using curryware_fantasy_command_line_tool.StatsCommands;
using curryware_fantasy_command_line_tool.PlayerCommands;
using Serilog;
using curryware_yahoo_parsing_library.FirebaseOAuthCallHandler;

namespace curryware_yahoo_api.Controllers;

[ApiController]
[Route("[controller]")]
public class YahooApiController : Controller
{
    
    [HttpGet(Name = "CallYahooApi")]
    public ActionResult<string> Index()
    {
        // CurrywareLogHandler.AddLog("Controller Result", LogLevel.Information);
        Log.Information("Calling Yahoo API");
        return "Controller Result";
    }

    [HttpGet]
    [Route("GetOAuthToken")]
    public async Task<IActionResult> GetOAuthToken()
    {
        // CurrywareLogHandler.AddLog("Calling Get OAuth Token", LogLevel.Information);
        Log.Information("Calling Get OAuth Token");
        try
        {
            var oauthTokenValue = await FirebaseOAuthCall.GetOAuthTokenFromFirebase();
            var tokenToLog = oauthTokenValue.Substring(0, 10);
            // CurrywareLogHandler.AddLog("OAuth token retrieved from Firebase: " + tokenToLog + "...", 
            //    LogLevel.Information);
            Log.Information("OAuth token retrieved from Firebase: " + tokenToLog + "...");
            return Ok(oauthTokenValue);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            // CurrywareLogHandler.AddLog("GetOAuthToken failed: " + e.Message, LogLevel.Error);
            Log.Error("GetOAuthToken failed: " + e.Message);
            return Ok("Error");
        }
    }

    // [HttpGet]
    // [Route("GetLeagueStandings")]
    // public async Task<IActionResult> GetLeagueStandings()
    // {
    //     foreach (var header in Request.Headers)
    //     {
    //         CurrywareLogHandler.AddLog($"{header.Key}: {header.Value}", LogLevel.Information);
    //     }
    //     
    //     // Hard-coded for now.  Need to fix
    //     var gameId = 449;
    //     var leagueId = 483521;
    //     
    //     try
    //     {
    //         var leagueStandingClass = new LeagueStandings();
    //         var leagueStandings = await leagueStandingClass.GetLeagueStandings(gameId, leagueId);
    //         var jsonHandler = new DictionaryJsonHandler();
    //         var json = jsonHandler.DictionaryToJsonString(leagueStandings);
    //         return Ok(json);
    //     }
    //     catch (HttpRequestException ex)
    //     {
    //         CurrywareLogHandler.AddLog("HttpRequestException: " + ex.Message, LogLevel.Error);
    //         return Unauthorized();
    //     }
    // }
    
    // [HttpGet]
    // [Route("GetAllPlayers")]
    // public async Task<IActionResult> GetLeagueInfo()
    // {
    //     var gameId = 449;
    //     var leagueId = 483521;
    //
    //     Log.Information($"Calling GetAllPlayers for gameId: {gameId}, leagueId: {leagueId}");
    //     try
    //     {
    //         var token = await FirebaseOAuthCall.GetOAuthTokenFromFirebase();
    //         var playersApi = new GetAllPlayersApi();
    //         var totalPlayers = await playersApi.GetAllPlayers(gameId, leagueId, token);
    //         return Ok(totalPlayers);
    //     }
    //     catch (HttpRequestException ex)
    //     {
    //         CurrywareLogHandler.AddLog("HttpRequestException: " + ex.Message, LogLevel.Error);
    //         return Unauthorized();
    //     }
    // }
    
    [HttpGet]
    [Route("LoadPlayerStatistics")]
    public async Task<IActionResult> LoadPlayerStatistics(int leagueId, int gameId, int weekNumber, string position)
    {
        var queryParameters = Request.Query;
        var statsCommandModelDictionary = new Dictionary<string, string?>()
        {
            { "leagueId", queryParameters["leagueId"] },
            { "gameId", queryParameters["gameId"]},
            { "week", queryParameters["weekNumber"]},
            { "position", queryParameters["position"]}
        };

        var gameStatsCommandLine = CommandLineParser.GameStatsParametersRest(statsCommandModelDictionary);
        Log.Debug($"gameid: {gameId}, week: {weekNumber}, position: {position}, leagueid: {leagueId}");
        // CurrywareLogHandler.AddLog($"Command Line: ", LogLevel.Debug);
        var returnValue = await StatsCommand.GetStats(gameStatsCommandLine);
        var totalBatches = returnValue.Count;
        return Ok(totalBatches.ToString());
    }

    [HttpGet]
    [Route("LoadPlayerInfo")]
    public async Task<IActionResult> LoadPlayerInfo([Required]int leagueId, [Required] int gameId, string position = "None", string status = "None")
    {
        Log.Information($"Calling LoadPlayerInfo for gameId: {gameId}, leagueId: {leagueId}");
        var queryParameters = Request.Query;
        var playerCommandDictionary = new Dictionary<string, string?>()
        {
            { "leagueId", queryParameters["leagueId"] },
            { "gameId", queryParameters["gameId"] },
            { "status", queryParameters["status"]},
            { "position", queryParameters["position"]}
        };
        
        var debugString = $"LoadPlayerInfo Parameters: {leagueId}, {gameId}, {status}, {position}";
        //CurrywareLogHandler.AddLog(debugString, LogLevel.Debug);
        Log.Debug(debugString);
        var playerCommandLineParameters = CommandLineParser.PlayerLoadParametersRest(playerCommandDictionary);
        var totalBatches = await PlayerCommand.RunGetPlayersCommand(playerCommandLineParameters);
        
        return Ok(totalBatches);
    }
}