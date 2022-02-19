namespace Nordigen.Net.Internal;

using Model;
using Responses;
using Queries;
using System;
using System.Threading;
using System.Threading.Tasks;

internal class AccountsEndpoint : IAccountsEndpoint
{
    private readonly INordigenHttpClient _client;

    public AccountsEndpoint(INordigenHttpClient client)
    {
        _client = client;
    }

    public Task<NOneOf<Account, Error>> Get(Guid id, CancellationToken cancellationToken) => _client.Get<Account>($"api/v2/accounts/{id}/", cancellationToken);

    public async Task<NOneOf<AccountDetails, Error>> Details(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _client.Get<AccountDetailsHolder>($"api/v2/accounts/{id}/details/", cancellationToken);
        return result.Match(x => NOneOf<AccountDetails, Error>.FromT0(x.Account), _ => _);
    }

    public async Task<NOneOf<Balance[], Error>> Balances(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _client.Get<BalancesHolder>($"api/v2/accounts/{id}/balances/", cancellationToken);
        return result.Match(x => NOneOf<Balance[], Error>.FromT0(x.Balances), _ => _);
    }

    public async Task<NOneOf<Transactions, Error>> Transactions(Guid id, AccountTransactionsFilter filter, CancellationToken cancellationToken = default)
    {
        var result = await _client.Get<TransactionsHolder>($"api/v2/accounts/{id}/transactions/?date_from={filter.DateFrom:yyyy-MM-dd}&date_to={filter.DateTo:yyyy-MM-dd}", cancellationToken);
        return result.Match(x => NOneOf<Transactions, Error>.FromT0(x.Transactions), _ => _);
    }
}