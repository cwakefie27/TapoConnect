using System.Text.Json.Serialization;

namespace TapoConnect.Dto
{
    public class CloudListDeviceReponse : TapoResponse<CloudListDeviceResult>
    {
    }

    public class CloudListDeviceResult
    {
        [JsonPropertyName("deviceList")]
        public List<TapoDeviceDto> DeviceList { get; set; } = null!;
    }
}
