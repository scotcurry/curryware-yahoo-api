using System.Net;
using System.Web;
using Serilog;

namespace curryware_yahoo_parsing_library.HttpHandler;

public abstract class HttpRequestHandler
{
    public static async Task<string> MakeYahooApiCall(string endPoint, string oauthToken)
    {
        Log.Debug($"Making Yahoo API Call: {endPoint}");
        try
        {
            var bearerToken = "Bearer " + oauthToken;
            var urlBuilder = new UriBuilder(endPoint);
            Log.Debug($"Making Yahoo API Call: {urlBuilder}");
            
            var parameters = HttpUtility.ParseQueryString(urlBuilder.Query);
            parameters["format"] = "xml";
            urlBuilder.Query = parameters.ToString();
            
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Add("Authorization", bearerToken);

            var response = await httpClient.GetAsync(urlBuilder.ToString());

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return result;
            }
            Log.Debug($"Response from Yahoo API: {response.StatusCode}");
            Log.Debug($"Yahoo API URI: {urlBuilder.Uri.ToString()}");
            
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                // CurrywareLogHandler.AddLog("Unauthorized", LogLevel.Error);
                Log.Error("Unauthorized Call in MakeYahooApiCall");
                return "Error: Unauthorized";
            }

            if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                // CurrywareLogHandler.AddLog("Forbidden", LogLevel.Error);
                Log.Error("Forbidden Call in MakeYahooApiCall");
                return "Error: Forbidden";
            }

            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                // CurrywareLogHandler.AddLog("Internal Server Error", LogLevel.Error);
                Log.Error("Internal Server Error in MakeYahooApiCall");
                return "Error: Internal Server Error";
            }
            
            // CurrywareLogHandler.AddLog("Uncaught Error in MakeYahooApiCall", LogLevel.Error);
            Log.Error("Uncaught Error in MakeYahooApiCall");
            return "Error: Uncaught Error in MakeYahooApiCall";
        }
        catch (InvalidOperationException invalidOperationException)
        {
            // CurrywareLogHandler.AddLog(invalidOperationException.Message, LogLevel.Error);
            Log.Error(invalidOperationException.Message);
            return "Error: InvalidOperation Error";
        }
        catch (HttpRequestException httpRequestException)
        {
            // CurrywareLogHandler.AddLog(httpRequestException.Message, LogLevel.Error);
            Log.Error(httpRequestException.Message);
            return "Error: HttpRequestException Error";
        }
        catch (TaskCanceledException taskCanceledException)
        {
            // CurrywareLogHandler.AddLog(taskCanceledException.Message, LogLevel.Error);
            Log.Error(taskCanceledException.Message);
            return "Error: TaskCanceledException Error";
        }
    }
}