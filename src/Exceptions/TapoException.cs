namespace TapoConnect.Exceptions
{
    public class TapoException : Exception
    {
        public const int SuccessErrorCode = 0;

        public const int RequestMethodNotSupportedErrorCode = -10000;
        public const int InvalidPubliKeyLengthErrorCode = -1010;
        public const int InvalidRequestOrCredentialsErrorCode = -1501;
        public const int IncorrectRequestErrorCode = -1002;
        public const int JsonFormatErrorCode = -1003;
        public const int ParameterDoesntExistErrorCode = -20104;
        public const int CloudTokenExpiredOrInvalidErrorCode = -20675;
        public const int DeviceTokenExpiredOrInvalidErrorCode = 9999;
        public const int TokenExpiredErrorCode = -20651;
        public const int HttpResponseErrorCode = -10000;
        public const int SecurePassthroughDepreacted = 1003;

        public static void ThrowFromErrorCode(int errorCode)
        {
            switch (errorCode)
            {
                case SuccessErrorCode: return;

                case RequestMethodNotSupportedErrorCode: throw new TapoInvalidRequestException(errorCode, "Request method not supported");
                case InvalidPubliKeyLengthErrorCode: throw new TapoInvalidRequestException(errorCode, "Invalid public key length");
                case InvalidRequestOrCredentialsErrorCode: throw new TapoInvalidRequestException(errorCode, "Invalid request or credentials");
                case ParameterDoesntExistErrorCode: throw new TapoInvalidRequestException(errorCode, "Parameter doesn't exist");
                case IncorrectRequestErrorCode: throw new TapoInvalidRequestException(errorCode, "Incorrect request");
                case JsonFormatErrorCode: throw new TapoJsonException("JSON format error");
                case CloudTokenExpiredOrInvalidErrorCode: throw new TapoCloudTokenExpiredOrInvalidException("Cloud token expired or invalid");
                case DeviceTokenExpiredOrInvalidErrorCode: throw new TapoDeviceTokenExpiredOrInvalidException("Device token expired or invalid");
                case TokenExpiredErrorCode: throw new TapoTokenExpiredException("Token expired");
                case SecurePassthroughDepreacted: throw new TapoSecurePassThroughProtocolDeprecatedException("Secure passthrough protocol deperecated in firmware >= \"1.1.0 Build 230721 Rel.224802\" for KLAP.");
                default: throw new TapoException(errorCode, $"Unexpected Error Code: {errorCode}");
            }
        }

        public int ErrorCode { get; }

        public TapoException(int errorCode, string? message) : base(message)
        {
            ErrorCode = errorCode;
        }

        public TapoException(string? message) : base(message)
        {
        }
    }
}
