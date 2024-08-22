using Microsoft.AspNetCore.Mvc;
using Serilog;
using Serilog.Formatting.Json;
using curryware_yahoo_api.OAuthModels;

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
}