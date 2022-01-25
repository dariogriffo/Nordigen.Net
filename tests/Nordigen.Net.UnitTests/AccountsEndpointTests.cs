using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Nordigen.Net.Internal;
using Nordigen.Net.Responses;
using RichardSzalay.MockHttp;
using Xunit;

namespace Nordigen.Net.UnitTests
{
    public class AccountsEndpointTests
    {
        private static readonly NordigenApiOptions Options = new NordigenApiOptions()
        {
            Url = "https://api.nordigen.com/"
        };

        [Fact]
        public async Task Get_When_Account_Is_Found_Returns_Valid_Information()
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
            var factory = Mock.Of<IHttpClientFactory>();
            Mock.Get(factory).Setup(x => x.CreateClient("api")).Returns(client);
            var sut = new AccountsEndpoint(new NordigenHttpClient(tokensEndpoint, factory, Options));

            var result = await sut.Get(id, CancellationToken.None);

            var expected = new Account(
                "3fa85f64-5717-4562-b3fc-2c963f66afa6",
                DateTime.Parse("2022-01-22T22:30:50.538Z"),
                DateTime.Parse("2022-01-22T22:30:50.538Z"),
                "NL49ABNA1969256915",
                "Lloyds",
                new Dictionary<string, string>() { { "DISCOVERED", "User has successfully authenticated and account is discovered" } });
            result.Value.Should().BeOfType<Account>();
            result.Value.Should().BeEquivalentTo(expected, x => x.Excluding(y => y.Created));
        }

        [Fact]
        public async Task Get_When_Account_Is_Not_Found_Returns_NotFound()
        {
            var id = Guid.NewGuid();
            var authToken = Guid.NewGuid().ToString();
            var handlerMock = new MockHttpMessageHandler();
            handlerMock
                .When($"/api/v2/accounts/{id}/")
                .With(x => x.Headers.Any(h => h.Key == "Authorization" && h.Value.First() == $"Bearer {authToken}"))
                .Respond(_ => new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new ReadOnlyMemoryContent(Encoding.UTF8.GetBytes("{\"status_code\": 404,\"summary\":\"Invalid Account ID\"}"))
                });

            var client = new HttpClient(handlerMock) { BaseAddress = new Uri(Options.Url) };
            var tokensEndpoint = Mock.Of<ITokensEndpoint>();
            Mock.Get(tokensEndpoint)
                .Setup(x => x.Get(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Token(authToken, 10, string.Empty, 10));
            var factory = Mock.Of<IHttpClientFactory>();
            Mock.Get(factory).Setup(x => x.CreateClient("api")).Returns(client);
            var sut = new AccountsEndpoint(new NordigenHttpClient(tokensEndpoint, factory, Options));

            var result = await sut.Get(id, CancellationToken.None);

            result
                .Value.Should().BeOfType<Error>()
                .And.Subject.As<Error>()
                .StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task Get_Server_Errors_Returns_UnknownRequestError()
        {
            var id = Guid.NewGuid();
            var authToken = Guid.NewGuid().ToString();
            var handlerMock = new MockHttpMessageHandler();
            handlerMock
                .When($"/api/v2/accounts/{id}/")
                .With(x => x.Headers.Any(h => h.Key == "Authorization" && h.Value.First() == $"Bearer {authToken}"))
                .Respond(_ => new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new ReadOnlyMemoryContent(Encoding.UTF8.GetBytes("{\"status_code\": 500,\"summary\":\"test\"}"))
                });

            var client = new HttpClient(handlerMock) { BaseAddress = new Uri(Options.Url) };
            var tokensEndpoint = Mock.Of<ITokensEndpoint>();
            Mock.Get(tokensEndpoint)
                .Setup(x => x.Get(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Token(authToken, 10, string.Empty, 10));

            var factory = Mock.Of<IHttpClientFactory>();
            Mock.Get(factory).Setup(x => x.CreateClient("api")).Returns(client);
            var sut = new AccountsEndpoint(new NordigenHttpClient(tokensEndpoint, factory, Options));
            var result = await sut.Get(id, CancellationToken.None);

            result
                .Value.Should().BeOfType<Error>()
                .And.Subject.As<Error>()
                .StatusCode.Should().Be(500);
        }

    }
}
