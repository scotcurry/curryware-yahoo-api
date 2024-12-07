using curryware_yahoo_api.FirebaseOAuthCallHandler;
using curryware_yahoo_api.JsonHandlers;
using curryware_yahoo_api.LogHandler;
using curryware_yahoo_api.PlayerApis;
using Microsoft.AspNetCore.Mvc;
using curryware_yahoo_api.TeamApis;

namespace curryware_yahoo_api.Controllers;

[ApiController]
[Route("[controller]")]
public class YahooApiController : Controller
{
    
    [HttpGet(Name = "CallYahooApi")]
    public ActionResult<String> Index()
    {
        CurrywareLogHandler.AddLog("Controller Result", LogLevel.Information);
        return "Controller Result";
    }

    [HttpGet]
    [Route("GetOAuthToken")]
    public async Task<IActionResult> GetOAuthToken()
    {
        CurrywareLogHandler.AddLog("Calling Get OAuth Token", LogLevel.Information);
        try
        {
            var oauthTokenValue = await FirebaseOAuthCall.GetOAuthTokenFromFirebase();
            var tokenToLog = oauthTokenValue.Substring(0, 10);
            CurrywareLogHandler.AddLog("OAuth token retrieved from Firebase: " + tokenToLog + "...", 
                LogLevel.Information);
            return Ok(oauthTokenValue);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            CurrywareLogHandler.AddLog("GetOAuthToken failed: " + e.Message, LogLevel.Error);
            return Ok("Error");
        }
    }

    [HttpGet]
    [Route("GetLeagueStandings")]
    public async Task<IActionResult> GetLeagueStandings()
    {
        foreach (var header in Request.Headers)
        {
            CurrywareLogHandler.AddLog($"{header.Key}: {header.Value}", LogLevel.Information);
        }
        
        // Hard-coded for now.  Need to fix
        var gameId = 449;
        var leagueId = 483521;
        
        try
        {
            var leagueStandingClass = new LeagueStandings();
            var leagueStandings = await leagueStandingClass.GetLeagueStandings(gameId, leagueId);
            var jsonHandler = new DictionaryJsonHandler();
            var json = jsonHandler.DictionaryToJsonString(leagueStandings);
            return Ok(json);
        }
        catch (HttpRequestException ex)
        {
            CurrywareLogHandler.AddLog("HttpRequestException: " + ex.Message, LogLevel.Error);
            return Unauthorized();
        }
    }
    
    [HttpGet]
    [Route("GetAllPlayers")]
    public async Task<IActionResult> GetLeagueInfo()
    {
        var gameId = 449;
        var leagueId = 483521;

        try
        {
            var token = await FirebaseOAuthCall.GetOAuthTokenFromFirebase();
            var playersApi = new GetAllPlayersApi();
            var totalPlayers = await playersApi.GetAllPlayers(gameId, leagueId, token);
            return Ok(totalPlayers);
        }
        catch (HttpRequestException ex)
        {
            CurrywareLogHandler.AddLog("HttpRequestException: " + ex.Message, LogLevel.Error);
            return Unauthorized();
        }
    }
}