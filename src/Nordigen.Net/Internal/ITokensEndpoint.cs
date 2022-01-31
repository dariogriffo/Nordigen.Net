using System.Threading;
using System.Threading.Tasks;

namespace Nordigen.Net.Internal
{
    using Responses;

    internal interface ITokensEndpoint
    {
        Task<NOneOf<Token, Error>> Get(CancellationToken cancellationToken = default);

        Task<NOneOf<Token, Error>> Refresh(string refresh, CancellationToken cancellationToken = default);
    }
}
