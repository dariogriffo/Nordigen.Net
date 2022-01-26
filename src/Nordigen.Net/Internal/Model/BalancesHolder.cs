namespace Nordigen.Net.Internal.Model
{
    using Newtonsoft.Json;
    using Responses;

    public class BalancesHolder
    {
        [JsonConstructor]
        public BalancesHolder(Balance[] balances)
        {
            Balances = balances;
        }

        public Balance[] Balances { get; }
    }
}

