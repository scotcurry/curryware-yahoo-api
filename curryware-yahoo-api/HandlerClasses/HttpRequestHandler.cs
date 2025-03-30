// using System.Web;
//
// namespace curryware_yahoo_api.HandlerClasses;
//
// public class HttpRequestHandler
// {
//     private const string BaseUri = "https://fantasysports.yahooapis.com/fantasy/v2";
//
//     public static async Task<string> MakeYahooApiCall(string endPoint, string oauthToken)
//     {
//         if (endPoint.Substring(0, 1) != "/")
//         {
//             endPoint = "/" + endPoint;
//         }
//         
//         var  urlToCall = BaseUri + endPoint;
//         var bearerToken = "Bearer " + oauthToken;
//        
//         var urlBuilder = new UriBuilder(urlToCall);
//         var parameters = HttpUtility.ParseQueryString(urlBuilder.Query);
//         parameters["format"] = "xml";
//         urlBuilder.Query = parameters.ToString();
//         
//         using var httpClient = new HttpClient();
//         httpClient.DefaultRequestHeaders.Clear();
//         httpClient.DefaultRequestHeaders.Add("Authorization", bearerToken);
//         
//         var response = await httpClient.GetAsync(urlBuilder.ToString());
//
//         if (response.IsSuccessStatusCode )
//         {
//             var result = await response.Content.ReadAsStringAsync();
//             return result;
//         }
//
//         throw new HttpRequestException($"Error on endpoint {endPoint}.  Status code: {response.StatusCode}");
//     }
//}