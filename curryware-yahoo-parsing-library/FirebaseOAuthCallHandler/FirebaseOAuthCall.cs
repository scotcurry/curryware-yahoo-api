using System.Net;
using System.Text.Json;

using curryware_yahoo_parsing_library.FirebaseModels;
using Serilog;

namespace curryware_yahoo_parsing_library.FirebaseOAuthCallHandler;

public abstract class FirebaseOAuthCall
{
    public static async Task<string> GetOAuthTokenFromFirebase()
    {
        var gcpUri = "https://curryware-firebase-auth-gcp-399646747702.us-central1.run.app/get_oauth_token";
        Log.Debug($"Getting OAuth Token from Firebase: {gcpUri}");

        using var client = new HttpClient();
        try
        {
            var currentUnixTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var validAuthTokenTime = currentUnixTime + 3500;
            var response = await client.GetAsync(gcpUri);
            Log.Debug($"Response from Firebase: {response.StatusCode}");

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var firebaseOAuth = JsonSerializer.Deserialize<FirebaseOAuthModel>(responseContent);
                if (firebaseOAuth == null) return firebaseOAuth!.AuthToken != null ? firebaseOAuth.AuthToken : "Error";

                var logString = "Last Update Time: " + firebaseOAuth.LastUpdateTime + ", Valid Time: " +
                                validAuthTokenTime;
                // CurrywareLogHandler.AddLog(logString, LogLevel.Information);
                Log.Debug(logString);
                if (firebaseOAuth.LastUpdateTime <= validAuthTokenTime)
                    return firebaseOAuth.AuthToken ?? "Error";

                await Task.Delay(250);
                responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                firebaseOAuth = JsonSerializer.Deserialize<FirebaseOAuthModel>(responseContent);
                return firebaseOAuth!.AuthToken != null ? firebaseOAuth.AuthToken : "Error";
            }

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                // CurrywareLogHandler.AddLog("Unauthorized", LogLevel.Error);
                Log.Error("Unauthorized");
                return "Error: Unauthorized";
            }

            if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                // CurrywareLogHandler.AddLog("Forbidden", LogLevel.Error);
                Log.Error("Forbidden");
                return "Error: Forbidden";
            }

            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                // CurrywareLogHandler.AddLog("Internal Server Error", LogLevel.Error);
                Log.Error("Internal Server Error");
                return "Error: Internal Server Error";
            }

            // CurrywareLogHandler.AddLog("Uncaught Error in GetOAuthTokenFromFirebase", LogLevel.Error);
            Log.Error("Uncaught Error in GetOAuthTokenFromFirebase");
            return "Error: Uncaught Error in GetOAuthTokenFromFirebase";
        }
        catch (InvalidOperationException invalidOperationException)
        {
            // CurrywareLogHandler.AddLog(invalidOperationException.Message, LogLevel.Error);
            Log.Error(invalidOperationException.Message);
            return "Error: InvalidOperation";
        }
        catch (HttpRequestException httpRequestException)
        {
            // CurrywareLogHandler.AddLog(httpRequestException.Message, LogLevel.Error);
            var debug = httpRequestException.InnerException?.Message;
            Log.Error(httpRequestException.Message);
            return "Error: HttpRequestException";
        }
        catch (TaskCanceledException taskCanceledException)
        {
            // CurrywareLogHandler.AddLog(taskCanceledException.Message, LogLevel.Error);
            Log.Error(taskCanceledException.Message);
            return "Error: TaskCanceledException";
        }
    }
}