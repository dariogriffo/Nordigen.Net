using System.Linq;

namespace Nordigen.Net.Responses
{
    using Newtonsoft.Json;

    public class Institution
    {
        [JsonConstructor]
        internal Institution(string id, string name, string bic, string transaction_total_days, string[] countries, string logo)
        {
            Id = id;
            Name = name;
            Bic = bic;
            TransactionTotalDays = transaction_total_days;
            Countries = countries.Distinct().ToArray();
            Logo = logo;
        }

        public string Id { get; }

        public string Name { get; }

        public string Bic { get; }

        public string TransactionTotalDays { get; }

        public string[] Countries { get; }

        public string Logo { get; }
    }
}
