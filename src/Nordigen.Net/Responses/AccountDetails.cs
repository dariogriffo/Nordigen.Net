namespace Nordigen.Net.Responses;

using Newtonsoft.Json;

public class AccountDetails
{
    [JsonConstructor]
    internal AccountDetails(
        string resourceId,
        string iban,
        string currency,
        string? ownerName,
        string? name,
        string? product,
        string? cashAccountType)
    {
        ResourceId = resourceId;
        IBAN = iban;
        Currency = currency;
        OwnerName = ownerName;
        Name = name;
        Product = product;
        CashAccountType = cashAccountType;
    }

    public string ResourceId { get; }

    public string IBAN { get; }

    public string Currency { get; }

    public string? OwnerName { get; }

    public string? Name { get; }

    public string? Product { get; }

    public string? CashAccountType { get; }
}
