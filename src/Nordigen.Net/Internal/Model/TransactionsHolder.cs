namespace Nordigen.Net.Internal.Model;

using Newtonsoft.Json;
using Responses;

internal class TransactionsHolder
{
    [JsonConstructor]
    public TransactionsHolder(Transactions transactions)
    {
        Transactions = transactions;
    }

    public Transactions Transactions { get; }
}