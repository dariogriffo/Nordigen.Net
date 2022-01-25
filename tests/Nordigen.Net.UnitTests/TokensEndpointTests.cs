using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Nordigen.Net.Internal;
using RichardSzalay.MockHttp;
using Xunit;

namespace Nordigen.Net.UnitTests
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    public class TokensEndpointTests
    {
        private static readonly JsonSerializerSettings Settings = new JsonSerializerSettings()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        private static readonly NordigenApiOptions Options = new NordigenApiOptions()
        {
            Url = "https://api.nordigen.com/",
            SecretId = Guid.NewGuid().ToString()
        };

        [Fact]
        public async Task Get_When_Request_Is_Valid_Returns_Correct_Response()
        {
            var authToken = Guid.NewGuid().ToString();
            var handlerMock = new MockHttpMessageHandler();
            var expected = new Token(string.Empty, 10, string.Empty, 10);
            handlerMock
                .When(HttpMethod.Post, "/api/v2/token/new/")
                .With(x => x.Headers.Any() == false)
                .With(x => ((StringContent)x.Content!).ReadAsStringAsync().Result.Contains(Options.SecretId))
                .Respond("application/json", $"{JsonConvert.SerializeObject(expected, Settings)}");

            var client = new HttpClient(handlerMock) { BaseAddress = new Uri(Options.Url) };
            var tokensEndpoint = Mock.Of<ITokensEndpoint>();
            Mock.Get(tokensEndpoint)
                .Setup(x => x.Get(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Token(authToken, 10, string.Empty, 10));

            var factory = Mock.Of<IHttpClientFactory>();
            Mock.Get(factory).Setup(x => x.CreateClient("tokens")).Returns(client);

            var sut = new TokensEndpoint(factory, Options);

            var result = await sut.Get(CancellationToken.None);

            result.Value.Should().BeOfType<Token>();
            result.Value.Should().BeEquivalentTo(expected);
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
                .Respond("application/json",
                    $"{JsonConvert.SerializeObject(expected, Settings)}"
                );

            var client = new HttpClient(handlerMock) { BaseAddress = new Uri(Options.Url) };
            var tokensEndpoint = Mock.Of<ITokensEndpoint>();
            Mock.Get(tokensEndpoint)
                .Setup(x => x.Get(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Token(authToken, 10, string.Empty, 10));
            var factory = Mock.Of<IHttpClientFactory>();
            Mock.Get(factory).Setup(x => x.CreateClient("tokens")).Returns(client);

            var sut = new TokensEndpoint(factory, Options);

            var result = await sut.Refresh(refreshToken, CancellationToken.None);

            result.Value.Should().BeOfType<Token>();
            result.Value.Should().BeEquivalentTo(expected);
        }
    }
}
