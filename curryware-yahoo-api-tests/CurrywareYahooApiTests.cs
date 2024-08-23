using Xunit;

using Serilog;
using Serilog.Formatting.Json;

using curryware_yahoo_api.HandlerClasses;
using curryware_yahoo_api.FirebaseOAuthCallHandler;

namespace curryware_yahoo_api_tests;

public class CurrywareYahooApiTests
{
    [Fact]
    public static async Task TestFireBaseOAuthCall()
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console(new JsonFormatter())
            .CreateLogger(); ;

        var oauthToken = await FirebaseOAuthCall.GetOAuthTokenFromFirebase();
        var tokenToPrint = oauthToken.Substring(0, 10);
        Log.Information("OAuth token: {tokenToPrint}", tokenToPrint);
        Assert.NotEqual("Error", oauthToken);
    }
    
    [Fact]
    public static async Task TestHttpRequestHandler()
    {
        var expectedResult = "<";
        var oauthToken = await FirebaseOAuthCall.GetOAuthTokenFromFirebase();
        
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console(new JsonFormatter())
            .CreateLogger();
        Log.Information("OAuth token: {OAuthToken}", oauthToken);

        var result = await HttpRequestHandler.MakeYahooApiCall("team/406.l.967657.t.4/roster",
            oauthToken);
        var firstCharacter = result.Substring(0, 1);
        Assert.Equal(expectedResult, firstCharacter);
    }
}