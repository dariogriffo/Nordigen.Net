namespace Nordigen.Net.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Moq;
    using Nordigen.Net.Internal;
    using Nordigen.Net.Responses;
    using RichardSzalay.MockHttp;
    using Xunit;

    public class NordigenHttpClientTests
    {
        private static readonly NordigenApiOptions Options = new NordigenApiOptions();
        private readonly ISerializer _serializer = new Serializer();


        [Fact]
        public async Task Get_When_Request_Is_Valid_Returns_Correct_Response()
        {
            var id = Guid.NewGuid();
            var authToken = Guid.NewGuid().ToString();
            var handlerMock = new MockHttpMessageHandler();
            handlerMock
                .When($"/api/v2/accounts/{id}/")
                .With(x => x.Headers.Any(h => h.Key == "Authorization" && h.Value.First() == $"Bearer {authToken}"))
                .Respond("application/json",
                    "{\"id\": \"3fa85f64-5717-4562-b3fc-2c963f66afa6\",\"created\": \"2022-01-22T22:30:50.538Z\",\"last_accessed\": \"2022-01-22T22:30:50.538Z\",\"iban\": \"NL49ABNA1969256915\",\"institution_id\": \"Lloyds\",\"status\": {\"DISCOVERED\": \"User has successfully authenticated and account is discovered\"}}"
                );

            var client = new HttpClient(handlerMock) { BaseAddress = new Uri(Options.Url) };
            var tokensEndpoint = Mock.Of<ITokensEndpoint>();
            Mock.Get(tokensEndpoint)
                .Setup(x => x.Get(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Token(authToken, 10, string.Empty, 10));


            var expected = new Account(
                "3fa85f64-5717-4562-b3fc-2c963f66afa6",
                DateTime.Parse("2022-01-22T22:30:50.538Z"),
                DateTime.Parse("2022-01-22T22:30:50.538Z"),
                "NL49ABNA1969256915",
                "Lloyds",
                new Dictionary<string, string>() { { "DISCOVERED", "User has successfully authenticated and account is discovered" } });

            var factory = Mock.Of<IHttpClientFactory>();
            Mock.Get(factory).Setup(x => x.CreateClient("api")).Returns(client);
            var sut = new NordigenHttpClient(tokensEndpoint, factory, _serializer, Options);

            var result = await sut.Get<Account>($"/api/v2/accounts/{id}/", CancellationToken.None);

            result.Value.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task Get_When_Called_Twice_And_Token_Is_Still_Valid_Reuses_Same_Token()
        {
            var id = Guid.NewGuid();
            var authToken = Guid.NewGuid().ToString();
            var handlerMock = new MockHttpMessageHandler();
            handlerMock
                .When($"/api/v2/accounts/{id}/")
                .With(x => x.Headers.Any(h => h.Key == "Authorization" && h.Value.First() == $"Bearer {authToken}"))
                .Respond("application/json",
                    "{\"id\": \"3fa85f64-5717-4562-b3fc-2c963f66afa6\",\"created\": \"2022-01-22T22:30:50.538Z\",\"last_accessed\": \"2022-01-22T22:30:50.538Z\",\"iban\": \"NL49ABNA1969256915\",\"institution_id\": \"Lloyds\",\"status\": {\"DISCOVERED\": \"User has successfully authenticated and account is discovered\"}}"
                );

            var client = new HttpClient(handlerMock) { BaseAddress = new Uri(Options.Url) };
            var tokensEndpoint = Mock.Of<ITokensEndpoint>();
            var tokensEndpointMock = Mock.Get(tokensEndpoint);
            tokensEndpointMock
                .Setup(x => x.Get(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Token(authToken, 10, string.Empty, 10));

            var factory = Mock.Of<IHttpClientFactory>();
            Mock.Get(factory).Setup(x => x.CreateClient("api")).Returns(client);
            var sut = new NordigenHttpClient(tokensEndpoint, factory, _serializer, Options);

            _ = await sut.Get<Account>($"/api/v2/accounts/{id}/", CancellationToken.None);

            _ = await sut.Get<Account>($"/api/v2/accounts/{id}/", CancellationToken.None);

            tokensEndpointMock
                .Verify(x => x.Get(It.IsAny<CancellationToken>()), Times.Exactly(1));
        }


        [Fact]
        public async Task Get_When_Called_Twice_And_Token_Is_Not_Valid_But_Refresh_Is_Valid_Refreshes_Token()
        {
            var refreshToken = Guid.NewGuid().ToString();
            var id = Guid.NewGuid();
            var authToken = Guid.NewGuid().ToString();
            var handlerMock = new MockHttpMessageHandler();
            handlerMock
                .When($"/api/v2/accounts/{id}/")
                .With(x => x.Headers.Any(h => h.Key == "Authorization" && h.Value.First() == $"Bearer {authToken}"))
                .Respond("application/json",
                    "{\"id\": \"3fa85f64-5717-4562-b3fc-2c963f66afa6\",\"created\": \"2022-01-22T22:30:50.538Z\",\"last_accessed\": \"2022-01-22T22:30:50.538Z\",\"iban\": \"NL49ABNA1969256915\",\"institution_id\": \"Lloyds\",\"status\": {\"DISCOVERED\": \"User has successfully authenticated and account is discovered\"}}"
                );

            var client = new HttpClient(handlerMock) { BaseAddress = new Uri(Options.Url) };
            var tokensEndpoint = Mock.Of<ITokensEndpoint>();
            var tokensEndpointMock = Mock.Get(tokensEndpoint);
            tokensEndpointMock
                .Setup(x => x.Get(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Token(authToken, 1, refreshToken, 1000));

            var lastResponse = new Token(authToken, 10, refreshToken, 1000);
            tokensEndpointMock
                .Setup(x => x.Refresh(refreshToken, It.IsAny<CancellationToken>()))
                .ReturnsAsync(NOneOf<Token, Error>.FromT0(lastResponse));

            var options = new NordigenApiOptions()
            {
                Url = "https://api.nordigen.com/",
                AccessTokenValidBeforeSeconds = 1
            };

            var factory = Mock.Of<IHttpClientFactory>();
            Mock.Get(factory).Setup(x => x.CreateClient("api")).Returns(client);
            var sut = new NordigenHttpClient(tokensEndpoint, factory, _serializer, options);

            _ = await sut.Get<Account>($"/api/v2/accounts/{id}/", CancellationToken.None);

            Thread.Sleep(TimeSpan.FromSeconds(2));

            _ = await sut.Get<Account>($"/api/v2/accounts/{id}/", CancellationToken.None);

            tokensEndpointMock
                .Verify(x => x.Get(It.IsAny<CancellationToken>()), Times.Exactly(1));

            tokensEndpointMock
                .Verify(x => x.Refresh(refreshToken, It.IsAny<CancellationToken>()), Times.Exactly(1));

            sut.Token.Should().BeEquivalentTo(lastResponse);
        }
    }
}
