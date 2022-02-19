using Newtonsoft.Json;

namespace Nordigen.Net.Responses;

public class Deleted
{
    [JsonConstructor]
    public Deleted(
        string summary,
        string detail
    )
    {
        this.Summary = summary;
        this.Detail = detail;
    }

    public string Summary { get; }
        
    public string Detail { get; }
}