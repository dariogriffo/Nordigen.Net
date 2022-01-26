namespace Nordigen.Net.Responses
{
    using Newtonsoft.Json;

    public class AccountDetails
    {
        [JsonConstructor]
        internal AccountDetails(string resource_id, string iban, string currency, string owner_name, string name, string product, string cash_account_type)
        {
            ResourceId = resource_id;
            IBAN = iban;
            Currency = currency;
            OwnerName = owner_name;
            Name = name;
            Product = product;
            CashAccountType = cash_account_type;
        }

        public string ResourceId { get; }

        public string IBAN { get; }

        public string Currency { get; }

        public string OwnerName { get; }

        public string Name { get; }

        public string Product { get; }

        public string CashAccountType { get; }
    }
}
