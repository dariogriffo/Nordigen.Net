namespace Nordigen.Net
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Nordigen.Net.Responses;
    using OneOf;
    using Queries;

    public interface IAccountsEndpoint
    {
        /// <summary>
        /// Access account metadata.
        /// Information about the account record, such as the processing status and IBAN.
        /// Account status is recalculated based on the error count in the latest req.
        /// </summary>
        /// <param name="id">The id of the account</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/></param>
        /// <returns>The <see cref="Account"/> or <see cref="Error"/></returns>
        /// <returns></returns>
        Task<OneOf<Account, Error>> Get(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        ///Access account details.
        /// Account details will be returned in Berlin Group PSD2 format.
        /// </summary>
        /// <param name="id">The id of the account</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/></param>
        /// <returns>A list of <see cref="AccountDetails"/> or <see cref="Error"/></returns>
        Task<OneOf<AccountDetails, Error>> Details(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Access account balances.
        /// Balances will be returned in Berlin Group PSD2 format.
        /// </summary>
        /// <param name="id">The id of the account</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/></param>
        /// <returns>A list of <see cref="Balance"/> or <see cref="Error"/></returns>
        Task<OneOf<Balance[], Error>> Balances(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Access account transactions by query.
        /// Transactions will be returned in Berlin Group PSD2 format.
        /// </summary>
        /// <param name="id">The id of the account</param>
        /// <param name="filter">The query to be applied <see cref="AccountTransactionsFilter"/></param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/></param>
        /// <returns><see cref="Transactions"/> or <see cref="Error"/></returns>
        Task<OneOf<Transactions, Error>> Transactions(Guid id, AccountTransactionsFilter filter, CancellationToken cancellationToken = default);
    }
}
