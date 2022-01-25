using System;
using System.Threading;
using System.Threading.Tasks;
using Nordigen.Net.Responses;
using OneOf;

namespace Nordigen.Net.Internal
{
    internal class AccountsEndpoint : IAccountsEndpoint
    {
        private readonly INordigenHttpClient _client;

        public AccountsEndpoint(INordigenHttpClient client)
        {
            _client = client;
        }

        public Task<OneOf<Account, Error>> Get(Guid id, CancellationToken cancellationToken) => _client.Get<Account>($"api/v2/accounts/{id}/", cancellationToken);
    }
}
