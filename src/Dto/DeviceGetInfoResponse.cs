using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace TapoConnect.Dto
{
    public class DeviceGetInfoResponse : TapoResponse<DeviceGetInfoResult>
    {
    }

    public class DeviceGetInfoResult
    {
        [JsonPropertyName("device_id")]
        public string DeviceId { get; set; } = null!;

        [JsonPropertyName("hw_ver")]
        public string HwVersion { get; set; } = null!;

        [JsonPropertyName("fw_ver")]
        public string FwVersion { get; set; } = null!;

        [JsonPropertyName("type")]
        public string Type { get; set; } = null!;

        [JsonPropertyName("model")]
        public string Model { get; set; } = null!;

        [JsonPropertyName("mac")]
        public string Mac { get; set; } = null!;

        [JsonPropertyName("hw_id")]
        public string HwId { get; set; } = null!;

        [JsonPropertyName("fw_id")]
        public string FwId { get; set; } = null!;

        [JsonPropertyName("oem_id")]
        public string OemId { get; set; } = null!;

        [JsonPropertyName("color_temp_range")]
        public List<int> ColorTemperatureRange { get; set; } = null!;

        [JsonPropertyName("overheated")]
        public bool Overheated { get; set; }

        [JsonPropertyName("Ip")]
        public string IpAddress { get; set; } = null!;

        [JsonPropertyName("time_diff")]
        public int TimeDiff { get; set; }

        [JsonPropertyName("ssid")]
        public string Ssid { get; set; } = null!;

        [JsonPropertyName("rssi")]
        public int Rssi { get; set; }

        [JsonPropertyName("signal_level")]
        public int SignalLevel { get; set; }

        [JsonPropertyName("latitude")]
        public int Latitude { get; set; }

        [JsonPropertyName("longitude")]
        public int Longitude { get; set; }

        [JsonPropertyName("lang")]
        public string Lang { get; set; } = null!;

        [JsonPropertyName("avatar")]
        public string Avatar { get; set; } = null!;

        [JsonPropertyName("region")]
        public string Region { get; set; } = null!;

        [JsonPropertyName("specs")]
        public string Specs { get; set; } = null!;

        [JsonPropertyName("nickname")]
        public string Nickname { get; set; } = null!;

        [JsonPropertyName("has_set_location_info")]
        public bool DeviceOn { get; set; }

        [JsonPropertyName("brightness")]
        public int Brightness { get; set; }

        [JsonPropertyName("hue")]
        public int Hue { get; set; }

        [JsonPropertyName("saturation")]
        public int Saturation { get; set; }

        [JsonPropertyName("color_temp")]
        public int ColorTemperature { get; set; }

        [JsonPropertyName("dynamic_light_effect_enable")]
        public bool DynamicLightEffectEnable { get; set; }

        [JsonPropertyName("default_states")]
        public DeviceGetInfoDefaultStateDto DefaultState { get; set; } = null!;

    }

    public class DeviceGetInfoDefaultStateDto
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = null!;

        [JsonPropertyName("state")]
        public JsonObject State { get; set; } = null!;
    }
}
