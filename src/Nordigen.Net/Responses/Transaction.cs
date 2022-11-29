namespace Nordigen.Net.Responses;

using Newtonsoft.Json;
using System;

public class Transaction
{
    [JsonConstructor]
    internal Transaction(
        string? transactionId,
        string? bankTransactionCode,
        string? proprietaryBankTransactionCode,
        string? endToEndId,
        string? checkId,
        string? internalTransactionId,
        string? mandateId,
        string? entryReference,
        TransactionAmount transactionAmount,
        string? debtorName,
        AccountIdentification? debtorAccount,
        string? ultimateDebtor,
        string? creditorName,
        AccountIdentification? creditorAccount,
        string? creditorId,
        string? ultimateCreditor,
        DateTime? bookingDate,
        DateTime? bookingDateTime,
        DateTime? valueDate,
        DateTime? valueDateTime,
        string? remittanceInformationUnstructured,
        string? additionalInformation,
        string? remittanceInformationStructured)
    {
        TransactionId = transactionId;
        BankTransactionCode = bankTransactionCode;
        ProprietaryBankTransactionCode = proprietaryBankTransactionCode;
        EndToEndId = endToEndId;
        CheckId = checkId;
        InternalTransactionId = internalTransactionId;
        MandateId = mandateId;
        EntryReference = entryReference;
        TransactionAmount = transactionAmount;
        DebtorName = debtorName;
        DebtorAccount = debtorAccount;
        UltimateDebtor = ultimateDebtor;
        CreditorName = creditorName;
        CreditorAccount = creditorAccount;
        CreditorId = creditorId;
        UltimateCreditor = ultimateCreditor;
        BookingDate = bookingDate;
        BookingDateTime = bookingDateTime;
        ValueDate = valueDate;
        ValueDateTime = valueDateTime;
        RemittanceInformationUnstructured = remittanceInformationUnstructured;
        AdditionalInformation = additionalInformation;
        RemittanceInformationStructured = remittanceInformationStructured;
    }
    public string? TransactionId { get; }
    public string? BankTransactionCode { get; }
    public string? ProprietaryBankTransactionCode { get; }
    public string? EndToEndId { get; }
    public string? CheckId { get; }
    public string? InternalTransactionId { get; }
    public string? MandateId { get; }
    public string? EntryReference { get; }
    public TransactionAmount TransactionAmount { get; }

    public string? DebtorName { get; }
    public AccountIdentification? DebtorAccount { get; }
    public string? UltimateDebtor { get; }
    
    public string? CreditorName { get; }
    public AccountIdentification? CreditorAccount { get; }
    public string? CreditorId { get; }
    public string? UltimateCreditor { get; }

    public DateTime? BookingDate { get; }
    public DateTime? BookingDateTime { get; set; }
    public DateTime? ValueDate { get; }
    public DateTime? ValueDateTime { get; }
    
    public string? RemittanceInformationUnstructured { get; }
    public string? AdditionalInformation { get; }
    public string? RemittanceInformationStructured { get; } 
}
