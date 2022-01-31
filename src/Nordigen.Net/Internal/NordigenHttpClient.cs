namespace Nordigen.Net.Internal
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Responses;

    internal class NordigenHttpClient : INordigenHttpClient
    {
        internal const string HttpClientName = "api";

        private readonly ITokensEndpoint _tokensEndpoint;
        private readonly ISerializer _serializer;
        private readonly HttpClient _client;
        private readonly NordigenApiOptions _options;
        private readonly object _lock = new object();
        private Token? _token;

        public NordigenHttpClient(
            ITokensEndpoint tokensEndpoint,
            IHttpClientFactory factory,
            ISerializer serializer,
            NordigenApiOptions options)
        {
            _client = factory.CreateClient(HttpClientName);
            _options = options;
            _serializer = serializer;
            _tokensEndpoint = tokensEndpoint;
        }

        public async Task<NOneOf<T, Error>> Get<T>(string url, CancellationToken cancellationToken)
            where T : class
        {
            try
            {
                await EnsureValidToken(cancellationToken);
            }
            catch (UnauthorizedAccessException)
            {
                return new Error(401, string.Empty, string.Empty, string.Empty);
            }

            var message = await _client.GetAsync(url, cancellationToken);
            return message.IsSuccessStatusCode
                ? (NOneOf<T, Error>)_serializer.Deserialize<T>(await message.Content.ReadAsStringAsync())
                : _serializer.Deserialize<Error>(await message.Content.ReadAsStringAsync());
        }

        public async Task<NOneOf<TResult, Error>> Post<T, TResult>(T model, string url, CancellationToken cancellationToken = default) where T : class
        {
            try
            {
                await EnsureValidToken(cancellationToken);
            }
            catch (UnauthorizedAccessException)
            {
                return new Error(401, string.Empty, string.Empty, string.Empty);
            }

            var content = new StringContent(_serializer.Serialize(model), Encoding.UTF8, "application/json");

            var message = await _client.PostAsync(url, content, cancellationToken);
            return message.IsSuccessStatusCode
                ? (NOneOf<TResult, Error>)_serializer.Deserialize<TResult>(await message.Content.ReadAsStringAsync())
                : _serializer.Deserialize<Error>(await message.Content.ReadAsStringAsync());
        }

        public async Task<NOneOf<Deleted, Error>> Delete(string url, CancellationToken cancellationToken = default)
        {
            try
            {
                await EnsureValidToken(cancellationToken);
            }
            catch (UnauthorizedAccessException)
            {
                return new Error(401, string.Empty, string.Empty, string.Empty);
            }

            var message = await _client.DeleteAsync(url, cancellationToken);
            return message.IsSuccessStatusCode ?
                NOneOf<Deleted, Error>.FromT0(Deleted.Value)
                : _serializer.Deserialize<Error>(await message.Content.ReadAsStringAsync());
        }

        internal Token? Token => _token;

        private async Task EnsureValidToken(CancellationToken cancellationToken)
        {
            async Task UpdateToken(NOneOf<Token, Error> oneOf)
            {
                await oneOf.Match(t =>
                {
                    lock (_lock)
                    {
                        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", t.Access);
                        _token = t;
                    }

                    return Task.CompletedTask;
                }, _ => throw new UnauthorizedAccessException());
            }

            if (_token is null == false)
            {
                if (_token.AccessIsValid(_options.AccessTokenValidBeforeSeconds))
                {
                    return;
                }

                if (_token.RefreshIsValid(_options.RefreshTokenValidBeforeSeconds))
                {
                    var refresh = await _tokensEndpoint.Refresh(_token.Refresh, cancellationToken);
                    if (refresh.IsT0)
                    {
                        refresh.AsT0.Refresh = _token.Refresh;
                        refresh.AsT0.RefreshExpires = _token.RefreshExpires;
                    }

                    await UpdateToken(refresh);
                    return;
                }
            }

            var token = await _tokensEndpoint.Get(cancellationToken);
            await UpdateToken(token);

        }
    }
}
