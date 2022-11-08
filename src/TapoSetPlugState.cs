using System.Text.Json.Serialization;

namespace TapoConnect
{
    public class TapoSetPlugState : TapoSetDeviceState
    {
        public TapoSetPlugState(bool deviceOn)
        {
            DeviceOn = deviceOn;
        }
    }
}
