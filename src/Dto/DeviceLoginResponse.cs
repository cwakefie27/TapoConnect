using System.Text.Json.Serialization;

namespace TapoConnect.Dto
{
    public class DeviceLoginResponse : TapoResponse<DeviceLoginResult>
    {
    }

    public class DeviceLoginResult
    {
        [JsonPropertyName("token")]
        public string Token { get; set; } = null!;
    }
}
