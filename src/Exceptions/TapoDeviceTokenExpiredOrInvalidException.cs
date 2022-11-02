namespace TapoConnect.Exceptions
{
    public class TapoDeviceTokenExpiredOrInvalidException : TapoException
    {
        public TapoDeviceTokenExpiredOrInvalidException(string? message) : base(DeviceTokenExpiredOrInvalidErrorCode, message)
        {
        }
    }
}
