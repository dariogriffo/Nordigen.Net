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
        int max_historical_days,
        int access_valid_for_days,
        List<string> access_scope,
        DateTime? accepted,
        string institution_id
    )
    {
        Id = id;
        Created = created;
        MaxHistoricalDays = max_historical_days;
        AccessValidForDays = access_valid_for_days;
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
    public DateTime? Accepted { get; }

    [JsonProperty("institution_id")] 
    public string InstitutionId { get; }
}
