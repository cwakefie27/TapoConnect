using System.Text.Json.Serialization;

namespace TapoConnect
{
    public abstract class TapoSetDeviceState
    {
        [JsonPropertyName("device_on")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? DeviceOn { get; set; }

        public static bool operator ==(TapoSetDeviceState obj1, TapoSetDeviceState obj2)
        {
            if (ReferenceEquals(obj1, obj2))
                return true;
            if (ReferenceEquals(obj1, null))
                return false;
            if (ReferenceEquals(obj2, null))
                return false;
            return obj1.Equals(obj2);
        }

        public static bool operator !=(TapoSetDeviceState obj1, TapoSetDeviceState obj2) => !(obj1 == obj2);
        public bool Equals(TapoSetDeviceState? other)
        {
            if (ReferenceEquals(other, null))
                return false;

            if (ReferenceEquals(this, other))
                return true;

            return DeviceOn == other.DeviceOn;
        }

        public override bool Equals(object? obj) => Equals(obj as TapoSetDeviceState);

        public override int GetHashCode()
        {
            unchecked
            {
                return DeviceOn.GetHashCode();
            }
        }
    }
}
