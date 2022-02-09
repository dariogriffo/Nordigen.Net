namespace Nordigen.Net.Queries
{
    using System;

    public class AccountTransactionsFilter
    {
        public AccountTransactionsFilter(DateTime dateFrom, DateTime dateTo)
        {
            DateFrom = dateFrom;
            DateTo = dateTo;
        }

        public DateTime DateFrom { get; }

        public DateTime DateTo { get; }
    }
}
