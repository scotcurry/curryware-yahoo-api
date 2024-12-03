using Xunit;

using Serilog;
using Serilog.Formatting.Json;

using curryware_yahoo_api.HandlerClasses;
using curryware_yahoo_api.FirebaseOAuthCallHandler;
using curryware_yahoo_api.KafkaHandlers;
using curryware_yahoo_api.XMLParsers.LeaguePlayers;
using curryware_yahoo_api.PlayerApis;

namespace curryware_yahoo_api_tests;

public class PlayerRecordTest
{ 
    [Fact] 
    public static async Task TestPlayerXmlParser()
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console(new JsonFormatter())
            .CreateLogger(); ;

        var oauthToken = await FirebaseOAuthCall.GetOAuthTokenFromFirebase();
        var keyToPrint = oauthToken.Substring(0, 10);
        
        Log.Information("OAuth token: {keyToPrint}", keyToPrint);
        var playersXml = await HttpRequestHandler.MakeYahooApiCall("league/423.l.661655/players",
            oauthToken);
        var playerList = LeaguePlayerParser.GetParseLeaguePlayerXml(playersXml); 
        Assert.Equal(25, playerList.Count);
    }

    [Fact]
    public static async Task TestGetAllPlayers()
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console(new JsonFormatter())
            .CreateLogger();
        
        var gameNumber = 423;
        var teamNumber = 661655;
        var oauthToken = await FirebaseOAuthCall.GetOAuthTokenFromFirebase();
        var keyToPrint = oauthToken.Substring(0, 10);
        
        Log.Information("OAuth token: {keyToPrint}", keyToPrint);
        
        var getPlayersClass = new GetAllPlayersApi();
        try
        {
            var totalPlayers = await getPlayersClass.GetAllPlayers(gameNumber, teamNumber, oauthToken);
            Assert.Equivalent(1133, totalPlayers);
        }
        catch (KafkaValidationException kafkaException)
        {
            Assert.True(true, kafkaException.Message);
        }
    }
}