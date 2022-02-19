namespace Nordigen.Net.Responses;

using Newtonsoft.Json;

public class DebtorAccount
{

    [JsonConstructor]
    public DebtorAccount(string iban)
    {
        IBAN = iban;
    }

    public string IBAN { get; }
}