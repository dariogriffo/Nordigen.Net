﻿namespace Nordigen.Net.Internal
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading;
    using System.Threading.Tasks;
    using OneOf;
    using Responses;

    internal class NordigenHttpClient : INordigenHttpClient
    {
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
            _client = factory.CreateClient("api");
            _options = options;
            _serializer = serializer;
            _tokensEndpoint = tokensEndpoint;
        }

        public async Task<OneOf<T, Error>> Get<T>(string url, CancellationToken cancellationToken)
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
                ? (OneOf<T, Error>)_serializer.Deserialize<T>(await message.Content.ReadAsStringAsync())
                : _serializer.Deserialize<Error>(await message.Content.ReadAsStringAsync());
        }

        internal Token? Token => _token;

        private async Task EnsureValidToken(CancellationToken cancellationToken)
        {
            async Task UpdateToken(OneOf<Token, Error> oneOf)
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