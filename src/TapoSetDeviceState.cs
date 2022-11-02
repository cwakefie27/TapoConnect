using System.Text.Json.Serialization;

namespace TapoConnect
{
    public abstract class TapoSetDeviceState
    {
        [JsonPropertyName("device_on")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? DeviceOn { get; set; }
    }
}
