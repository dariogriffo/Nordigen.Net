namespace Nordigen.Net.Responses;

using Newtonsoft.Json;

public class BalanceAmount
{
    [JsonConstructor]
    internal BalanceAmount(string amount, string currency)
    {
        Amount = amount;
        Currency = currency;
    }

    public string Amount { get; }

    public string Currency { get; }

}
