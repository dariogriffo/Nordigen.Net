namespace Nordigen.Net.Internal
{
    using Responses;
    using System.Threading;
    using System.Threading.Tasks;

    internal interface INordigenHttpClient
    {
        Task<NOneOf<T, Error>> Get<T>(string url, CancellationToken cancellationToken = default)
            where T : class;

        Task<NOneOf<TResult, Error>> Post<T, TResult>(T model, string url, CancellationToken cancellationToken = default)
            where T : class;

        Task<NOneOf<Deleted, Error>> Delete(string url, CancellationToken cancellationToken = default);
    }
}
