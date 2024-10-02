using curryware_yahoo_api.JsonHandlers;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Serilog.Formatting.Json;
using curryware_yahoo_api.OAuthModels;
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

    [HttpPost]
    public IActionResult GetLeagueInfo([FromBody] OAuthTokenModel oauthToken)
    {

        var oauthTokenValue = oauthToken.OAuthToken;

        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console(new JsonFormatter())
            .CreateLogger();
        
        
        var tokenToLog = oauthTokenValue?.Substring(0, 10);
        Log.Information("OAuth Token Value: " + tokenToLog);
        return Ok();
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