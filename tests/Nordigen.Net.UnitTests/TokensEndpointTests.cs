namespace Nordigen.Net.UnitTests
{
    using FluentAssertions;
    using Moq;
    using Nordigen.Net.Internal;
    using RichardSzalay.MockHttp;
    using System;
    using System.Linq;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Xunit;

    public class TokensEndpointTests
    {
        private static readonly NordigenApiOptions Options = new NordigenApiOptions()
        {
            SecretId = Guid.NewGuid().ToString()
        };

        private readonly ISerializer _serializer = new Serializer();

        [Fact]
        public async Task Get_When_Request_Is_Valid_Returns_Correct_Response()
        {
            const string url = "/api/v2/token/new/";

            var expected = new Token(string.Empty, 10, string.Empty, 10);

            var authToken = Guid.NewGuid().ToString();
            var handlerMock = new MockHttpMessageHandler();
            handlerMock
                .When(HttpMethod.Post, url)
                .With(x => x.Headers.Any() == false)
                .With(x => ((StringContent)x.Content!).ReadAsStringAsync().Result.Contains(Options.SecretId))
                .Respond("application/json", _serializer.Serialize(expected));

            var client = new HttpClient(handlerMock) { BaseAddress = new Uri(Options.Url) };
            var tokensEndpoint = Mock.Of<ITokensEndpoint>();
            Mock.Get(tokensEndpoint)
                .Setup(x => x.Get(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Token(authToken, 10, string.Empty, 10));

            var factory = Mock.Of<IHttpClientFactory>();
            Mock.Get(factory).Setup(x => x.CreateClient("tokens")).Returns(client);

            var sut = new TokensEndpoint(factory, _serializer, Options);

            var result = await sut.Get(CancellationToken.None);

            result.AsT0.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task Refresh_When_Request_Is_Valid_Returns_Correct_Response()
        {
            var refreshToken = Guid.NewGuid().ToString();
            var authToken = Guid.NewGuid().ToString();
            var handlerMock = new MockHttpMessageHandler();
            var expected = new Token(string.Empty, 10, string.Empty, 10);
            handlerMock
                .When(HttpMethod.Post, "/api/v2/token/refresh/")
                .With(x => x.Headers.Any() == false)
                .With(x => ((StringContent)x.Content!).ReadAsStringAsync().Result.Contains(refreshToken))
                .Respond("application/json", _serializer.Serialize(expected));

            var client = new HttpClient(handlerMock) { BaseAddress = new Uri(Options.Url) };
            var tokensEndpoint = Mock.Of<ITokensEndpoint>();
            Mock.Get(tokensEndpoint)
                .Setup(x => x.Get(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Token(authToken, 10, string.Empty, 10));
            var factory = Mock.Of<IHttpClientFactory>();
            Mock.Get(factory).Setup(x => x.CreateClient("tokens")).Returns(client);

            var sut = new TokensEndpoint(factory, _serializer, Options);

            var result = await sut.Refresh(refreshToken, CancellationToken.None);

            result.AsT0.Should().BeEquivalentTo(expected);
        }
    }
}
