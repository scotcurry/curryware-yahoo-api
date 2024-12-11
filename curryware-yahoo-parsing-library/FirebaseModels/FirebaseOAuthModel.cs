using System.Text.Json.Serialization;

namespace curryware_yahoo_parsing_library.FirebaseModels;

internal class FirebaseOAuthModel
{
    [JsonPropertyName("oauth_token")]
    public string? AuthToken { get; set; }
    [JsonPropertyName("refresh_token")]
    public string? RefreshToken { get; set; }
    [JsonPropertyName("last_update_time")]
    public int LastUpdateTime { get; set; }
}