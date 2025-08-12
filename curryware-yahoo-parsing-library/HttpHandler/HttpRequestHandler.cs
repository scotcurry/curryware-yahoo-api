
using System.Net;
using System.Web;
using Serilog;

namespace curryware_yahoo_parsing_library.HttpHandler;

public abstract class HttpRequestHandler
{
    public static async Task<string> MakeYahooApiCall(string endPoint, string oauthToken)
    {
        using var httpClient = new HttpClient();
        return await MakeYahooApiCall(endPoint, oauthToken, httpClient);
    }

    // New overload that accepts HttpClient for testing
    private static async Task<string> MakeYahooApiCall(string endPoint, string oauthToken, HttpClient httpClient)
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
                Log.Error("Unauthorized Call in MakeYahooApiCall");
                return "Error: Unauthorized";
            }

            if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                Log.Error("Forbidden Call in MakeYahooApiCall");
                return "Error: Forbidden";
            }

            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                Log.Error("Internal Server Error in MakeYahooApiCall");
                return "Error: Internal Server Error";
            }
            
            Log.Error("Uncaught Error in MakeYahooApiCall");
            return "Error: Uncaught Error in MakeYahooApiCall";
        }
        catch (InvalidOperationException invalidOperationException)
        {
            Log.Error(invalidOperationException.Message);
            return "Error: InvalidOperation Error";
        }
        catch (HttpRequestException httpRequestException)
        {
            Log.Error(httpRequestException.Message);
            return "Error: HttpRequestException Error";
        }
        catch (TaskCanceledException taskCanceledException)
        {
            Log.Error(taskCanceledException.Message);
            return "Error: TaskCanceledException Error";
        }
    }
}