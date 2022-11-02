namespace TapoConnect.Exceptions
{
    public class TapoCloudTokenExpiredOrInvalidException : TapoException
    {
        public TapoCloudTokenExpiredOrInvalidException(string? message) : base(CloudTokenExpiredOrInvalidErrorCode, message)
        {
        }
    }
}
