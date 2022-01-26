namespace Nordigen.Net.Responses
{
    using Newtonsoft.Json;

    public class BalanceAccount
    {
        [JsonConstructor]
        internal BalanceAccount(string amount, string currency)
        {
            Amount = amount;
            Currency = currency;
        }

        public string Amount { get; }

        public string Currency { get; }

    }
}
