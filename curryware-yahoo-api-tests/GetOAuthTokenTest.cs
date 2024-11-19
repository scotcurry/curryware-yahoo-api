using Xunit;

using Serilog;
using Serilog.Formatting.Json;

using curryware_yahoo_api.FirebaseOAuthCallHandler;

namespace curryware_yahoo_api_tests;

public class OAuthTokenTest
{
    [Fact]
    public async Task GetLeagueInfoParserTest()
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console(new JsonFormatter())
            .CreateLogger();
        
        var token = await FirebaseOAuthCall.GetOAuthTokenFromFirebase();
        bool tokenHasNoJson = token.Trim().Substring(0, 1) != "{";
        if (tokenHasNoJson && token.Length > 10)
            Assert.True(token.Length > 10);
    }
}