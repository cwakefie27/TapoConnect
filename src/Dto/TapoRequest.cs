using System.Text.Json.Serialization;

namespace TapoConnect.Dto
{
    public class TapoRequest
    {
        [JsonPropertyName("method")]
        public string Method { get; set; } = null!;
    }

    public class TapoRequest<TParams> : TapoRequest
        where TParams : class
    {
        [JsonPropertyName("params")]
        public TParams Params { get; set; } = null!;
    }
}
