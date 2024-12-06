using System.Text.Json;
using curryware_yahoo_api.LogHandler;
using curryware_yahoo_api.OAuthModels;

namespace curryware_yahoo_api.FirebaseOAuthCallHandler;

public class FirebaseOAuthCall
{
    public static async Task<string> GetOAuthTokenFromFirebase()
    {
        var gcpUri = "https://curryware-firebase-auth-gcp-399646747702.us-central1.run.app/get_oauth_token";

        using var client = new HttpClient();
        var response = await client.GetAsync(gcpUri);
        
        var currentUnixTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        var validAuthTokenTime = currentUnixTime + 3500;
        
        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var firebaseOAuth = JsonSerializer.Deserialize<FirebaseOAuthModel>(responseContent);
            if (firebaseOAuth == null) return firebaseOAuth!.AuthToken != null ? firebaseOAuth.AuthToken : "Error";

            var logString = "Last Update Time: " + firebaseOAuth.LastUpdateTime + ", Valid Time: " + validAuthTokenTime;
            CurrywareLogHandler.AddLog(logString, LogLevel.Information);
            if (firebaseOAuth.LastUpdateTime <= validAuthTokenTime)
                return firebaseOAuth!.AuthToken != null ? firebaseOAuth.AuthToken : "Error";
            
            await Task.Delay(250);
            responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            firebaseOAuth = JsonSerializer.Deserialize<FirebaseOAuthModel>(responseContent);
            return firebaseOAuth!.AuthToken != null ? firebaseOAuth.AuthToken : "Error";
        }
        else
        {
            CurrywareLogHandler.AddLog("Error Getting OAuth Token", LogLevel.Error);
            return "Error";
        }
    }
}