using System.Collections.Generic;
using Newtonsoft.Json;

namespace Nordigen.Net.Requests;

public class Agreement
{
    [JsonConstructor]
    public Agreement(
        string institutionId,
        int maxHistoricalDays,
        int accessValidForDays,
        List<string> accessScope
    )
    {
        MaxHistoricalDays = maxHistoricalDays;
        AccessValidForDays = accessValidForDays;
        AccessScope = accessScope;
        InstitutionId = institutionId;
    }

    [JsonProperty("max_historical_days")]
    public int MaxHistoricalDays { get; }

    [JsonProperty("access_valid_for_days")]
    public int AccessValidForDays { get; }

    [JsonProperty("access_scope")]
    public IReadOnlyList<string> AccessScope { get; }
    
    [JsonProperty("institution_id")]
    public string InstitutionId { get; }
}