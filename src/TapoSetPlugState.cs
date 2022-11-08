using System.Text.Json.Serialization;

namespace TapoConnect
{
    public class TapoSetPlugState : TapoSetDeviceState, IEquatable<TapoSetDeviceState>
    {
        public TapoSetPlugState(bool deviceOn)
        {
            DeviceOn = deviceOn;
        }

        public static bool operator ==(TapoSetPlugState lhs, TapoSetPlugState rhs)
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

        public static bool operator !=(TapoSetPlugState obj1, TapoSetPlugState obj2) => !(obj1 == obj2);

        public bool Equals(TapoSetPlugState? other)
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

        public override bool Equals(object? obj) => Equals(obj as TapoSetPlugState);

        public override int GetHashCode()
        {
            unchecked
            {
                return DeviceOn.GetHashCode();
            }
        }
    }
}
