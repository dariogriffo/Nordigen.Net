namespace Nordigen.Net.Internal;

using Microsoft.AspNetCore.WebUtilities;
using Model;
using Responses;
using Queries;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

internal class AccountsEndpoint : IAccountsEndpoint, IEndpoint
{
    private readonly INordigenHttpClient _client;

    public AccountsEndpoint(INordigenHttpClient client)
    {
        _client = client;
    }

    public Task<NOneOf<Account, Error>> Get(string id, CancellationToken cancellationToken) =>
        _client.Get<Account>($"api/v2/accounts/{id}/", cancellationToken);

    public async Task<NOneOf<AccountDetails, Error>> Details(string id, CancellationToken cancellationToken = default)
    {
        var result = await _client.Get<AccountDetailsHolder>($"api/v2/accounts/{id}/details/", cancellationToken);
        return result.Match(x => NOneOf<AccountDetails, Error>.FromT0(x.Account), _ => _);
    }

    public async Task<NOneOf<Balance[], Error>> Balances(string id, CancellationToken cancellationToken = default)
    {
        var result = await _client.Get<BalancesHolder>($"api/v2/accounts/{id}/balances/", cancellationToken);
        return result.Match(x => NOneOf<Balance[], Error>.FromT0(x.Balances), _ => _);
    }

    public async Task<NOneOf<Transactions, Error>> Transactions(
        string id,
        AccountTransactionsFilter filter,
        CancellationToken cancellationToken = default)
    {
        var parameters = new Dictionary<string, string>();

        if (filter.DateFrom is { } dateFrom)
            parameters.Add("date_from", dateFrom.ToString("yyyy-MM-dd"));
        if (filter.DateTo is { } dateTo)
            parameters.Add("date_to", dateTo.ToString("yyyy-MM-dd"));

        var url = QueryHelpers.AddQueryString($"api/v2/accounts/{id}/transactions/", parameters);
        var result = await _client.Get<TransactionsHolder>(url, cancellationToken);

        return result.Match(x => NOneOf<Transactions, Error>.FromT0(x.Transactions), _ => _);
    }
}
