using curryware_yahoo_api.FirebaseOAuthCallHandler;
using curryware_yahoo_api.JsonHandlers;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Serilog.Formatting.Json;
using curryware_yahoo_api.TeamApis;

namespace curryware_yahoo_api.Controllers;

[ApiController]
[Route("[controller]")]
public class YahooApiController : Controller
{
    
    [HttpGet(Name = "CallYahooApi")]
    public ActionResult<String> Index()
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console(new JsonFormatter())
            .CreateLogger();
        
        Log.Information("Call From CallYahooApi");
        return "Controller Result";
    }

    [HttpGet]
    [Route("GetOAuthToken")]
    public async Task<IActionResult> GetOAuthToken()
    {

        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console(new JsonFormatter())
            .CreateLogger();
        
        Log.Information("Calling Get OAuth Token");
        try
        {
            var oauthTokenValue = await FirebaseOAuthCall.GetOAuthTokenFromFirebase();
            var tokenToLog = oauthTokenValue.Substring(0, 10);
            Log.Information("OAuth token retrieved from Firebase: " + tokenToLog + "...");
            return Ok(oauthTokenValue);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            Log.Error(e, "GetOAuthToken failed: " + e.Message);
            return Ok("Error");
        }
    }

    [HttpGet]
    [Route("GetLeagueStandings")]
    public async Task<IActionResult> GetLeagueStandings()
    {
        // Hard-coded for now.  Need to fix
        var gameId = 449;
        var leagueId = 483521;
    
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console(new JsonFormatter())
            .CreateLogger();
    
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
            Log.Error(ex, "HttpRequestException: " + ex.Message);
            return Unauthorized();
        }
    }
}