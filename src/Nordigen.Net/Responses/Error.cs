namespace Nordigen.Net.Responses
{
    using Newtonsoft.Json;

    public class Error
    {
        [JsonConstructor]
        internal Error(int status_code, string summary, string detail, string? type)
        {
            Summary = summary;
            Detail = detail;
            Type = type;
            StatusCode = status_code;
        }

        public string Summary { get; }

        public string Detail { get; }
        
        public string? Type { get; }

        public int StatusCode { get; }
    }
}
