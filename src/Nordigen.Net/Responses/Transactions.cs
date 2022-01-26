namespace Nordigen.Net.Responses
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class Transactions
    {
        private const string BookedKey = "booked";
        private const string PendingKey = "pending";
        private readonly IDictionary<string, Transaction[]> _transactions;
        
        [JsonConstructor]
        internal Transactions(Transaction[] booked, Transaction[] pending)
        {
            _transactions = new Dictionary<string, Transaction[]>()
            {
                { BookedKey, booked},
                { PendingKey, pending}
            };
        }

        public Transaction[] Booked => _transactions[BookedKey];

        public Transaction[] Pending => _transactions[PendingKey];
    }
}
