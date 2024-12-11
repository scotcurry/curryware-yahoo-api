using System.Net;
using System.Web;
using Microsoft.Extensions.Logging;

using curryware_yahoo_parsing_library.LogHandler;

namespace curryware_yahoo_parsing_library.HttpHandler;

abstract class HttpRequestHandler
{
    internal static async Task<string> MakeYahooApiCall(string endPoint, string oauthToken)
    {
        try
        {
            var bearerToken = "Bearer " + oauthToken;
            var urlBuilder = new UriBuilder(endPoint);
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
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                CurrywareLogHandler.AddLog("Unauthorized", LogLevel.Error);
                return "Error: Unauthorized";
            }

            if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                CurrywareLogHandler.AddLog("Forbidden", LogLevel.Error);
                return "Error: Forbidden";
            }

            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                CurrywareLogHandler.AddLog("Internal Server Error", LogLevel.Error);
                return "Error: Internal Server Error";
            }
            
            CurrywareLogHandler.AddLog("Uncaught Error in MakeYahooApiCall", LogLevel.Error);
            return "Uncaught Error in MakeYahooApiCall";
        }
        catch (InvalidOperationException invalidOperationException)
        {
            CurrywareLogHandler.AddLog(invalidOperationException.Message, LogLevel.Error);
            return "InvalidOperation Error";
        }
        catch (HttpRequestException httpRequestException)
        {
            CurrywareLogHandler.AddLog(httpRequestException.Message, LogLevel.Error);
            return "HttpRequestException Error";
        }
        catch (TaskCanceledException taskCanceledException)
        {
            CurrywareLogHandler.AddLog(taskCanceledException.Message, LogLevel.Error);
            return "TaskCanceledException Error";
        }
    }
}