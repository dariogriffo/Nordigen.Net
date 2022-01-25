using System;
using System.Threading;
using System.Threading.Tasks;
using Nordigen.Net.Responses;
using OneOf;

namespace Nordigen.Net
{
    public interface IAccountsEndpoint
    {
        Task<OneOf<Account, Error>> Get(Guid id, CancellationToken cancellationToken = default);
    }
}
