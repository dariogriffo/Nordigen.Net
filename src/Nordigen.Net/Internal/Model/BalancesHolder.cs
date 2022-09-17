namespace Nordigen.Net.Internal.Model;

using Newtonsoft.Json;
using Responses;

internal class BalancesHolder
{
    [JsonConstructor]
    public BalancesHolder(Balance[] balances)
    {
        Balances = balances;
    }

    public Balance[] Balances { get; }
}