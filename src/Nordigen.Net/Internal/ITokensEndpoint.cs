using System.Threading;
using System.Threading.Tasks;
using OneOf;

namespace Nordigen.Net.Internal
{
    using Responses;

    internal interface ITokensEndpoint
    {
        Task<OneOf<Token, Error>> Get(CancellationToken cancellationToken = default);

        Task<OneOf<Token, Error>> Refresh(string refresh, CancellationToken cancellationToken = default);
    }
}
