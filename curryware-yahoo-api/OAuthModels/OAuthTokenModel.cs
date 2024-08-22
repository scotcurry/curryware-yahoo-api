using System.Text.Json.Serialization;

namespace curryware_yahoo_api.OAuthModels;

public class OAuthTokenModel
{
    [JsonPropertyName("oauth_token")]
    public string? OAuthToken { get; set; }
}