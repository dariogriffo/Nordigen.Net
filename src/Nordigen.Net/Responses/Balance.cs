namespace Nordigen.Net.Responses
{
    using System;
    using Newtonsoft.Json;

    public class Balance
    {
        [JsonConstructor]
        internal Balance(BalanceAccount balance_account, string balance_type, DateTime reference_date)
        {
            BalanceAccount = balance_account;
            BalanceType = balance_type;
            ReferenceDate = reference_date;
        }

        public BalanceAccount BalanceAccount { get; }
        
        public string BalanceType { get; }
        
        public DateTime ReferenceDate { get; }

    }
}
