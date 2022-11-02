using System.Text.Json.Serialization;

namespace TapoConnect.Dto
{
    public class CloudLoginResponse : TapoResponse<CloudLoginResult>
    {
    }

    public class CloudLoginResult
    {
        [JsonPropertyName("accountId")]
        public string AccountId { get; set; } = null!;

        [JsonPropertyName("regTime")]
        public DateTime RegTime { get; set; }

        [JsonPropertyName("countryCode")]
        public string CountryCode { get; set; } = null!;

        [JsonPropertyName("riskDetected")]
        public int RiskDetected { get; set; }

        [JsonPropertyName("nickname")]
        public string Nickname { get; set; } = null!;

        [JsonPropertyName("email")]
        public string Email { get; set; } = null!;

        [JsonPropertyName("token")]
        public string Token { get; set; } = null!;

        [JsonPropertyName("refreshToken")]
        public string? RefreshToken { get; set; }
    }
}
