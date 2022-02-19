namespace Nordigen.Net.Responses;

using Newtonsoft.Json;

public class TransactionAmount
{
    [JsonConstructor]
    public TransactionAmount(string currency, string amount)
    {
        Currency = currency;
        Amount = amount;
    }

    public string Currency { get; }

    public string Amount { get; }
}