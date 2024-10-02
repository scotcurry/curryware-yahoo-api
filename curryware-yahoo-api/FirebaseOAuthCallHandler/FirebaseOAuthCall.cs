using System.Text.Json;
using Serilog;
using Serilog.Formatting.Json;
using curryware_yahoo_api.OAuthModels;

namespace curryware_yahoo_api.FirebaseOAuthCallHandler;

public class FirebaseOAuthCall
{
    public static async Task<string> GetOAuthTokenFromFirebase()
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console(new JsonFormatter())
            .CreateLogger();
        
        var gcpUri = "https://us-central1-currywareff.cloudfunctions.net/curryware-firebase-auth";

        using var client = new HttpClient();
        var response = await client.GetAsync(gcpUri);
        
        var currentUnixTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        var validAuthTokenTime = currentUnixTime + 3500;
        
        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var firebaseOAuth = JsonSerializer.Deserialize<FirebaseOAuthModel>(responseContent);
            if (firebaseOAuth == null) return firebaseOAuth!.AuthToken != null ? firebaseOAuth.AuthToken : "Error";
            
            Log.Information("Last Update Time: {0}, Valid Time: {1}", firebaseOAuth.LastUpdateTime, validAuthTokenTime);
            if (firebaseOAuth.LastUpdateTime <= validAuthTokenTime)
                return firebaseOAuth!.AuthToken != null ? firebaseOAuth.AuthToken : "Error";
            
            await Task.Delay(250);
            responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            firebaseOAuth = JsonSerializer.Deserialize<FirebaseOAuthModel>(responseContent);
            return firebaseOAuth!.AuthToken != null ? firebaseOAuth.AuthToken : "Error";
        }
        else
        {
            return "Error";
        }
    }
}