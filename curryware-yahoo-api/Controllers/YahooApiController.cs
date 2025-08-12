using System.ComponentModel.DataAnnotations;
using curryware_fantasy_command_line_tool.CommandLineHandlers;
using curryware_fantasy_command_line_tool.PlayerCommands;
using curryware_fantasy_command_line_tool.StatsCommands;
using curryware_kafka_producer_library;
using curryware_yahoo_parsing_library.FirebaseOAuthCallHandler;
using curryware_yahoo_parsing_library.LeagueApis;
using Microsoft.AspNetCore.Mvc;
using Serilog;

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

    // This code just pulls an OAuthToken.  Used 
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

    [HttpGet]
    [Route("LoadStatInfo")]
    public async Task<IActionResult> LoadStatInfo([Required] int gameId, [Required] int leagueId)
    {
        Log.Information($"Calling LoadStatInfo for gameId: {gameId}, leagueId: {leagueId}");
        Log.Information($"LoadStatInfo Parameters: {leagueId}, {gameId}");
        var statsJson = await LeagueStatSettingsApi.GetLeagueScoringInformation(gameId, leagueId);

        if (statsJson != null)
        {
            try
            {
                Log.Debug($"Writing statsJson to JSON ");
                var kafkaTopic = "StatTopic";
                var kafkaResult = await KafkaProducer.CreateKafkaMessage(kafkaTopic, statsJson);
                if (kafkaResult)
                    Log.Information("Wrote JSON to StatTopic Queue");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        else
        {
            Log.Error("Error writing statsJson to JSON");
        }
        
        return Ok("statsJson");
    }

    [HttpGet]
    [Route("LoadStatValueInfo")]
    public async Task<IActionResult> LoadStatValueInfo([Required] int gameId, [Required] int leagueId)
    {
        Log.Information($"Calling LoadStatValueInfo for gameId: {gameId}, leagueId: {leagueId}");
        var statsValuesJson = await LeagueStatValueApi.GetLeagueStatValueInformation(gameId, leagueId);

        if (statsValuesJson != null)
        {
            try
            {
                Log.Debug("Writing statsValueJson to StatTopicValue Queue");
                var kafkaTopic = "StatValueTopic";
                var kafkaResult = await KafkaProducer.CreateKafkaMessage(kafkaTopic, statsValuesJson);
                if (kafkaResult)
                    Log.Information("Wrote JSON to StatTopicValue Queue");
            }
            catch (Exception ex)
            {
                Log.Error($"Error Writing to Kafka {ex.Message}");
            }
        }
        return Ok("statsValuesJson");
    }
    
    [HttpPost]
    [Route("AddToKafkaQueue")]
    public async Task<IActionResult> AddToKafkaQueue()
    {
        Log.Information("Adding to Kafka Queue");
        var kafkaTopic = "DatadogValidationTopic";
        
        using var reader = new StreamReader(Request.Body);
        var json = await reader.ReadToEndAsync();
        try
        {
            var kafkaResult = await KafkaProducer.CreateKafkaMessage(kafkaTopic, json);
            if (kafkaResult)
                Log.Information("Wrote JSON to DataValidation Queue");
        }
        catch (KafkaValidationException kafkaValidationException)
        {
            Log.Error(kafkaValidationException.Message);
            return Ok(kafkaValidationException.Message);
        }
        
        return Ok("Added to Kafka Queue");
    }
}
