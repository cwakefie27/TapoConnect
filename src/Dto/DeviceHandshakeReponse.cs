using System.Text.Json.Serialization;

namespace TapoConnect.Dto
{
    public class DeviceHandshakeReponse : TapoResponse<DeviceHandshakeResult>
    {
    }

    public class DeviceHandshakeResult
    {
        [JsonPropertyName("key")]
        public string Key { get; set; } = null!;
    }
}
