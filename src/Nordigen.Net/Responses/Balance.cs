namespace Nordigen.Net.Responses;

using Newtonsoft.Json;
using System;

public class Balance
{
    [JsonConstructor]
    internal Balance(
        BalanceAmount balanceAmount,
        string balanceType,
        DateTime referenceDate)
    {
        BalanceAmount = balanceAmount;
        BalanceType = balanceType;
        ReferenceDate = referenceDate;
    }

    public BalanceAmount BalanceAmount { get; }

    public string BalanceType { get; }

    public DateTime ReferenceDate { get; }

}
