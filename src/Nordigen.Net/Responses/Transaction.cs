namespace Nordigen.Net.Responses;

using Newtonsoft.Json;
using System;

public class Transaction
{
    [JsonConstructor]
    internal Transaction(
        string transactionId,
        string? debtorName,
        AccountIdentification? debtorAccount,
        string? creditorName,
        AccountIdentification? creditorAccount,
        TransactionAmount transactionAmount,
        string proprietaryBankTransactionCode,
        DateTime? bookingDate,
        DateTime? bookingDateTime,
        DateTime? valueDate,
        string endToEndId,
        string bankTransactionCode,
        string remittanceInformationUnstructured)
    {
        TransactionId = transactionId;
        DebtorName = debtorName;
        DebtorAccount = debtorAccount;
        CreditorName = creditorName;
        CreditorAccount = creditorAccount;
        TransactionAmount = transactionAmount;
        BankTransactionCode = bankTransactionCode;
        BookingDate = bookingDate;
        BookingDateTime = bookingDateTime;
        ValueDate = valueDate;
        EndToEndId = endToEndId;
        RemittanceInformationUnstructured = remittanceInformationUnstructured;
    }

    public string TransactionId { get; }

    public string? DebtorName { get; }

    public AccountIdentification? DebtorAccount { get; }
    public string? CreditorName { get; }
    public AccountIdentification? CreditorAccount { get; }

    public TransactionAmount TransactionAmount { get; }

    public string BankTransactionCode { get; }

    public DateTime? BookingDate { get; }

    public DateTime? ValueDate { get; }
    public string EndToEndId { get; }

    public string RemittanceInformationUnstructured { get; }
    public DateTime? BookingDateTime { get; set; }
}
