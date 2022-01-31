namespace Nordigen.Net.Internal
{
    using System.Threading;
    using System.Threading.Tasks;
    using Responses;

    internal interface INordigenHttpClient
    {
        Task<NOneOf<T, Error>> Get<T>(string url, CancellationToken cancellationToken = default)
            where T : class;
    }
}
