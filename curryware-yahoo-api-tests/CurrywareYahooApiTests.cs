using Xunit;

using curryware_yahoo_api.HandlerClasses;
using curryware_yahoo_api.FirebaseOAuthCallHandler;

namespace curryware_yahoo_api_tests;

public class CurrywareYahooApiTests
{
    [Fact]
    public static async Task TestFireBaseOAuthCall()
    {
        var oauthToken = await FirebaseOAuthCall.GetOAuthTokenFromFirebase();
        Assert.NotEqual("Error", oauthToken);
    }
    
    [Fact]
    public static async Task TestHttpRequestHandler()
    {
        var expectedResult = "<";
        var oauthToken = await FirebaseOAuthCall.GetOAuthTokenFromFirebase();

        var result = await HttpRequestHandler.MakeYahooApiCall("team/406.l.967657.t.4/roster",
            oauthToken);
        var firstCharacter = result.Substring(0, 1);
        Assert.Equal(expectedResult, firstCharacter);
    }
}