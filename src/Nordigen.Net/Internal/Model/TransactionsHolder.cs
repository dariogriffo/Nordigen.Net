namespace Nordigen.Net.Internal.Model;

using Newtonsoft.Json;
using Responses;

public class TransactionsHolder
{
    [JsonConstructor]
    public TransactionsHolder(Transactions transactions)
    {
        Transactions = transactions;
    }

    public Transactions Transactions { get; }
}