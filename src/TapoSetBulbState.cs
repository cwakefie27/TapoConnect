using System.Text.Json.Serialization;

namespace TapoConnect
{
    public class TapoSetBulbState : TapoSetDeviceState
    {
        public TapoSetBulbState(
            TapoColor color,
            int? brightness = null,
            bool? deviceOn = null)
        {
            DeviceOn = deviceOn;

            Hue = color?.Hue;
            Saturation = color?.Saturation;
            ColorTemperature = color?.ColorTemp;

            Brightness = brightness ?? color?.Brightness;
        }

        public TapoSetBulbState(
            int brightness,
            bool? deviceOn = null)
        {
            Brightness = brightness;
            DeviceOn = deviceOn;
        }

        public TapoSetBulbState(
            bool deviceOn)
        {
            DeviceOn = deviceOn;
        }

        [JsonPropertyName("brightness")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? Brightness { get; set; }

        [JsonPropertyName("hue")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? Hue { get; set; }

        [JsonPropertyName("saturation")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? Saturation { get; set; }

        [JsonPropertyName("color_temp")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? ColorTemperature { get; set; }
    }
}
