using System.Text.Json.Serialization;

namespace TapoConnect.Dto
{
    public class CloudRefreshLoginResponse : TapoResponse<CloudRefreshLoginResult>
    {
    }

    public class CloudRefreshLoginResult
    {
        [JsonPropertyName("token")]
        public string Token { get; set; } = null!;
    }
}
