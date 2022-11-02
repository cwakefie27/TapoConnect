namespace TapoConnect.Exceptions
{
    public class HttpResponseException : TapoException
    {
        public HttpResponseMessage Response { get; }

        public HttpResponseException(HttpResponseMessage response)
            : base(HttpResponseErrorCode, $"{response.StatusCode}: {response.ReasonPhrase}")
        {
            Response = response;
        }
    }
}
