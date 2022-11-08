using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace TapoConnect
{
    public abstract class TapoSetDeviceState : IEquatable<TapoSetDeviceState>
    {
        [JsonPropertyName("device_on")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? DeviceOn { get; set; }

        public static bool operator ==(TapoSetDeviceState lhs, TapoSetDeviceState rhs)
        {
            if (lhs is null)
            {
                if (rhs is null)
                {
                    return true;
                }

                return false;
            }

            return lhs.Equals(rhs);
        }

        public static bool operator !=(TapoSetDeviceState obj1, TapoSetDeviceState obj2) => !(obj1 == obj2);

        public bool Equals(TapoSetDeviceState? other)
        {
            if (other is null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (this.GetType() != other.GetType())
            {
                return false;
            }

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
