using System.Threading;
using System.Threading.Tasks;
using OneOf;

namespace Nordigen.Net.Internal
{
    using Responses;

    internal interface INordigenHttpClient
    {
        Task<OneOf<T, Error>> Get<T>(string url, CancellationToken cancellationToken = default)
            where T : class;
    }
}
