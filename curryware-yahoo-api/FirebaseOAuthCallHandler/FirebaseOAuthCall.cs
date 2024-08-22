using System.Text.Json;
using curryware_yahoo_api.OAuthModels;

namespace curryware_yahoo_api.FirebaseOAuthCallHandler;

public class FirebaseOAuthCall
{

    public static async Task<string> GetOAuthTokenFromFirebase()
    {
        var gcpUri = "https://us-central1-currywareff.cloudfunctions.net/curryware-firebase-auth";

        using var client = new HttpClient();
        var response = await client.GetAsync(gcpUri);
        
        var currentUnixTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        var validAuthTokenTime = currentUnixTime + 3500;
        
        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var firebaseOAuth = JsonSerializer.Deserialize<FirebaseOAuthModel>(responseContent);
            if (firebaseOAuth != null)
            {
                var lastUpdateTime = firebaseOAuth.LastUpdateTime;
                if (firebaseOAuth.LastUpdateTime > validAuthTokenTime)
                {
                    await Task.Delay(250);
                    responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    firebaseOAuth = JsonSerializer.Deserialize<FirebaseOAuthModel>(responseContent);
                }
            }
            return firebaseOAuth!.AuthToken != null ? firebaseOAuth.AuthToken : "Error";
        }
        else
        {
            return "Error";
        }
    }
    
}