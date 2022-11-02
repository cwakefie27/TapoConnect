using System.Text.Json.Serialization;

namespace TapoConnect.Dto
{
    public class TapoDeviceDto
    {
        [JsonPropertyName("deviceType")]
        public string DeviceType { get; set; } = null!;

        [JsonPropertyName("role")]
        public int Role { get; set; }

        [JsonPropertyName("fwVer")]
        public string FwVer { get; set; } = null!;

        [JsonPropertyName("appServerUrl")]
        public string AppServerUrl { get; set; } = null!;

        [JsonPropertyName("deviceRegion")]
        public string DeviceRegion { get; set; } = null!;

        [JsonPropertyName("deviceId")]
        public string DeviceId { get; set; } = null!;

        [JsonPropertyName("deviceName")]
        public string DeviceName { get; set; } = null!;

        [JsonPropertyName("deviceHwVer")]
        public string DeviceHwVer { get; set; } = null!;

        [JsonPropertyName("alias")]
        public string Alias { get; set; } = null!;

        [JsonPropertyName("deviceMac")]
        public string DeviceMac { get; set; } = null!;

        [JsonPropertyName("oemId")]
        public string OemId { get; set; } = null!;

        [JsonPropertyName("deviceModel")]
        public string DeviceModel { get; set; } = null!;

        [JsonPropertyName("hwId")]
        public string HwId { get; set; } = null!;

        [JsonPropertyName("fwId")]
        public string FwId { get; set; } = null!;

        [JsonPropertyName("isSameRegion")]
        public bool IsSameRegion { get; set; }

        [JsonPropertyName("status")]
        public int Status { get; set; }

    }
}
