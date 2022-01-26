namespace Nordigen.Net.Internal
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Model;
    using Nordigen.Net.Responses;
    using OneOf;
    using Queries;

    internal class AccountsEndpoint : IAccountsEndpoint
    {
        private readonly INordigenHttpClient _client;

        public AccountsEndpoint(INordigenHttpClient client)
        {
            _client = client;
        }

        public Task<OneOf<Account, Error>> Get(Guid id, CancellationToken cancellationToken) => _client.Get<Account>($"api/v2/accounts/{id}/", cancellationToken);
       
        public async Task<OneOf<AccountDetails, Error>> Details(Guid id, CancellationToken cancellationToken = default)
        {
            var result = await _client.Get<AccountDetailsHolder>($"api/v2/accounts/{id}/details/", cancellationToken);
            return result.Match(x => OneOf<AccountDetails, Error>.FromT0(x.Account), _ => _);
        }

        public async Task<OneOf<Balance[], Error>> Balances(Guid id, CancellationToken cancellationToken = default)
        {
            var result = await _client.Get<BalancesHolder>($"api/v2/accounts/{id}/balances/", cancellationToken);
            return result.Match(x => OneOf<Balance[], Error>.FromT0(x.Balances), _ => _);
        }

        public async Task<OneOf<Transactions, Error>> Transactions(Guid id, AccountTransactionsFilter filter, CancellationToken cancellationToken = default)
        {
            var result = await _client.Get<TransactionsHolder>($"api/v2/accounts/{id}/transactions/?date_from={filter.DateFrom:yyyy-MM-dd}&date_to={filter.DateTo:yyyy-MM-dd}", cancellationToken);
            return result.Match(x => OneOf<Transactions, Error>.FromT0(x.Transactions), _ => _);
        }
    }
}
