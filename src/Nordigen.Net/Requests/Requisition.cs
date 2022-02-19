using System;
using Newtonsoft.Json;

namespace Nordigen.Net.Requests;

public class Requisition
{
    public Requisition(
        string redirect,
        Guid institutionId,
        Guid agreement,
        string reference,
        string userLanguage,
        string ssn,
        bool accountSelection
    )
    {
        Redirect = redirect;
        InstitutionId = institutionId;
        Agreement = agreement;
        Reference = reference;
        UserLanguage = userLanguage;
        Ssn = ssn;
        AccountSelection = accountSelection;
    }

    [JsonProperty("redirect")]
    public string Redirect { get; }

    [JsonProperty("institution_id")]
    public Guid InstitutionId { get; }

    [JsonProperty("agreement")]
    public Guid Agreement { get; }

    [JsonProperty("reference")]
    public string Reference { get; }

    [JsonProperty("user_language")]
    public string UserLanguage { get; }

    [JsonProperty("ssn")]
    public string Ssn { get; }

    [JsonProperty("account_selection")]
    public bool AccountSelection { get; }
}