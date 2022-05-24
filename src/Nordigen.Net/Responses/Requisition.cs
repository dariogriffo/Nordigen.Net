namespace Nordigen.Net.Responses;

using Newtonsoft.Json;
using System;
using System.Collections.Generic;

public class Requisition
{
    [JsonConstructor]
    public Requisition(
        string id,
        DateTime? created,
        string? redirect,
        string status,
        string institutionId,
        string agreement,
        string reference,
        List<string> accounts,
        string userLanguage,
        string link,
        string ssn,
        bool accountSelection
    )
    {
        Id = id;
        Created = created;
        Redirect = redirect;
        Status = status;
        InstitutionId = institutionId;
        Agreement = agreement;
        Reference = reference;
        Accounts = accounts;
        UserLanguage = userLanguage;
        Link = link;
        Ssn = ssn;
        AccountSelection = accountSelection;
    }

    public string Id { get; }

    public DateTime? Created { get; }

    public string? Redirect { get; }

    public string Status { get; }

    public string InstitutionId { get; }

    public string Agreement { get; }

    public string Reference { get; }

    public IReadOnlyList<string> Accounts { get; }

    public string UserLanguage { get; }

    public string Link { get; }

    public string Ssn { get; }

    public bool AccountSelection { get; }
}
