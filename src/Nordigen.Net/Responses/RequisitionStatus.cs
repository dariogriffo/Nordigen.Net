namespace Nordigen.Net.Responses;

using Newtonsoft.Json;

public class RequisitionStatus
{
    [JsonConstructor]
    public RequisitionStatus(
        string @short,
        string @long,
        string description
    )
    {
        Short = @short;
        Long = @long;
        Description = description;
    }

    public string Short { get; }
    public string Long { get; }
    public string Description { get; }
}