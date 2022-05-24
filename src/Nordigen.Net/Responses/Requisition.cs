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
        string redirect,
        string status,
        string institution_Id,
        Guid agreement,
        string reference,
        List<string> accounts,
        string user_language,
        string link,
        string ssn,
        bool account_selection
    )
    {
        Id = id;
        Created = created;
        Redirect = redirect;
        Status = status;
        InstitutionId = institution_Id;
        Agreement = agreement;
        Reference = reference;
        Accounts = accounts;
        UserLanguage = user_language;
        Link = link;
        Ssn = ssn;
        AccountSelection = account_selection;
    }

    public string Id { get; }

    public DateTime? Created { get; }

    public string Redirect { get; }

    public string Status { get; }

    public string InstitutionId { get; }

    public Guid Agreement { get; }

    public string Reference { get; }

    public IReadOnlyList<string> Accounts { get; }

    public string UserLanguage { get; }

    public string Link { get; }

    public string Ssn { get; }

    public bool AccountSelection { get; }
}
