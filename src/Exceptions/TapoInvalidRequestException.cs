namespace TapoConnect.Exceptions
{
    public class TapoInvalidRequestException : TapoException
    {
        public TapoInvalidRequestException(int errorCode, string? message) : base(errorCode, message)
        {
        }
    }
}
