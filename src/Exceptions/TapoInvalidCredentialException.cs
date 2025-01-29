namespace TapoConnect.Exceptions
{
    public class TapoInvalidCredentialException : TapoException
    {
        public TapoInvalidCredentialException(string? message) : base(InvalidCredentialsErrorCode, message)
        {
        }
    }
}
