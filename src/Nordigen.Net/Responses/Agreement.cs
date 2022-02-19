using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Nordigen.Net.Responses;

public class Agreement
{
    [JsonConstructor]
    public Agreement(
        string id,
        DateTime created,
        int maxHistoricalDays,
        int accessValidForDays,
        List<string> access_scope,
        DateTime accepted,
        string institution_id
    )
    {
        Id = id;
        Created = created;
        MaxHistoricalDays = maxHistoricalDays;
        AccessValidForDays = accessValidForDays;
        AccessScope = access_scope;
        Accepted = accepted;
        InstitutionId = institution_id;
    }

    [JsonProperty("id")] 
    public string Id { get; }

    [JsonProperty("created")] 
    public DateTime Created { get; }

    [JsonProperty("max_historical_days")] 
    public int MaxHistoricalDays { get; }

    [JsonProperty("access_valid_for_days")]
    public int AccessValidForDays { get; }

    [JsonProperty("access_scope")] 
    public IReadOnlyList<string> AccessScope { get; }

    [JsonProperty("accepted")] 
    public DateTime Accepted { get; }

    [JsonProperty("institution_id")] 
    public string InstitutionId { get; }
}
