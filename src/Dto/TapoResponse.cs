using System.Text.Json.Serialization;

namespace TapoConnect.Dto
{
    public class TapoResponse
    {
        [JsonPropertyName("error_code")]
        public int ErrorCode { get; set; }
    }

    public abstract class TapoResponse<TResult> : TapoResponse
        where TResult : class
    {
        [JsonPropertyName("result")]
        public TResult Result { get; set; } = null!;
    }
}
