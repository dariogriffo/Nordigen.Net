using System;
using Newtonsoft.Json;

namespace Nordigen.Net.Requests;

public class Requisition
{
    public Requisition()
    {
    }

    public Requisition(
        string redirect,
        string institutionId,
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
    public string Redirect { get; set; }

    [JsonProperty("institution_id")]
    public string InstitutionId { get; set; }

    [JsonProperty("agreement")]
    public Guid Agreement { get; set; }

    [JsonProperty("reference")]
    public string Reference { get; set; }

    [JsonProperty("user_language")]
    public string UserLanguage { get; set; }

    [JsonProperty("ssn")]
    public string Ssn { get; set; }

    [JsonProperty("account_selection")]
    public bool AccountSelection { get; set; }
}
