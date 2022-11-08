using System.Text.Json.Serialization;

namespace TapoConnect
{
    public class TapoSetBulbState : TapoSetDeviceState
    {
        public TapoSetBulbState(
            TapoColor color,
            bool? deviceOn = null)
        {
            DeviceOn = deviceOn;

            Hue = color?.Hue;
            Saturation = color?.Saturation;
            ColorTemperature = color?.ColorTemp;

            Brightness = color?.Brightness;
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

        public static bool operator ==(TapoSetBulbState obj1, TapoSetBulbState obj2)
        {
            if (ReferenceEquals(obj1, obj2))
                return true;
            if (ReferenceEquals(obj1, null))
                return false;
            if (ReferenceEquals(obj2, null))
                return false;
            return obj1.Equals(obj2);
        }

        public static bool operator !=(TapoSetBulbState obj1, TapoSetBulbState obj2) => !(obj1 == obj2);
        public bool Equals(TapoSetBulbState? other)
        {
            if (ReferenceEquals(other, null))
                return false;

            if (ReferenceEquals(this, other))
                return true;

            return DeviceOn == other.DeviceOn &&
                Brightness == other.Brightness &&
                Hue == other.Hue &&
                Saturation == other.Saturation &&
                ColorTemperature == other.ColorTemperature;
        }
        public override bool Equals(object? obj) => Equals(obj as TapoSetBulbState);

        public override int GetHashCode()
        {
            unchecked
            {
                return (DeviceOn, Brightness, Hue, Saturation, ColorTemperature).GetHashCode();
            }
        }
    }
}
