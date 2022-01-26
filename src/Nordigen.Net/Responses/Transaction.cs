namespace Nordigen.Net.Responses
{
    using System;
    using Newtonsoft.Json;

    public class Transaction
    {
        [JsonConstructor]
        internal Transaction(
            string transactionId, 
            string debtorName, 
            DebtorAccount debtorAccount, 
            TransactionAmount transactionAmount,
            string bankTransactionCode,
            DateTime bookingDate, 
            DateTime valueDate,
            string remittanceInformationUnstructured)
        {
            TransactionId = transactionId;
            DebtorName = debtorName;
            DebtorAccount = debtorAccount;
            TransactionAmount = transactionAmount;
            BankTransactionCode = bankTransactionCode;
            BookingDate = bookingDate;
            ValueDate = valueDate;
            RemittanceInformationUnstructured = remittanceInformationUnstructured;
        }
        public string TransactionId { get; }
        
        public string DebtorName { get; }
        
        public DebtorAccount DebtorAccount { get; }
        
        public TransactionAmount TransactionAmount { get; }
        
        public string BankTransactionCode { get; }
        
        public DateTime BookingDate { get; }
        
        public DateTime ValueDate { get; }
        
        public string RemittanceInformationUnstructured { get; }
    }
}
