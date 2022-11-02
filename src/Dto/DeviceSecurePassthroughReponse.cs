using System.Text.Json.Serialization;

namespace TapoConnect.Dto
{
    public class DeviceSecurePassthroughReponse : TapoResponse<DeviceSecurePassthroughResult>
    {
    }

    public class DeviceSecurePassthroughResult
    {
        [JsonPropertyName("response")]
        public string Response { get; set; } = null!;
    }
}
