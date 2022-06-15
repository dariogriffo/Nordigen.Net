namespace Nordigen.Net.Responses;

using Newtonsoft.Json;

public class AccountIdentification
{
    [JsonConstructor]
    public AccountIdentification(string? iban, string? bban)
    {
        IBAN = iban;
        BBAN = bban;
    }

    public string? IBAN { get; }
    public string? BBAN { get; }
}
