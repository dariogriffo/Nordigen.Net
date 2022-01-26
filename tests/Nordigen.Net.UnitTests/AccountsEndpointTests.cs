namespace Nordigen.Net.UnitTests
{
    using System;
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
    using Queries;
    using RichardSzalay.MockHttp;
    using Xunit;

    public class AccountsEndpointTests
    {
        private static readonly NordigenApiOptions Options = new NordigenApiOptions();
        private readonly ISerializer _serializer = new Serializer();


        [Fact]
        public async Task Get_When_Account_Is_Found_Returns_Valid_Information()
        {
            var id = Guid.NewGuid();
            var authToken = Guid.NewGuid().ToString();
            var handlerMock = new MockHttpMessageHandler();
            const string content =
                "{\"id\": \"3fa85f64-5717-4562-b3fc-2c963f66afa6\",\"created\": \"2022-01-22T22:30:50.538Z\",\"last_accessed\": \"2022-01-22T22:30:50.538Z\",\"iban\": \"NL49ABNA1969256915\",\"institution_id\": \"Lloyds\",\"status\": {\"DISCOVERED\": \"User has successfully authenticated and account is discovered\"}}";
            handlerMock
                .When($"/api/v2/accounts/{id}/")
                .With(x => x.Headers.Any(h => h.Key == "Authorization" && h.Value.First() == $"Bearer {authToken}"))
                .Respond("application/json", content);

            var client = new HttpClient(handlerMock) { BaseAddress = new Uri(Options.Url) };
            var tokensEndpoint = Mock.Of<ITokensEndpoint>();
            Mock.Get(tokensEndpoint)
                .Setup(x => x.Get(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Token(authToken, 10, string.Empty, 10));
            var factory = Mock.Of<IHttpClientFactory>();
            Mock.Get(factory).Setup(x => x.CreateClient("api")).Returns(client);
            var serializer = _serializer;
            var sut = new AccountsEndpoint(new NordigenHttpClient(tokensEndpoint, factory, serializer, Options));

            var result = await sut.Get(id, CancellationToken.None);

            var expected = serializer.Deserialize<Account>(content);
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
            var sut = new AccountsEndpoint(new NordigenHttpClient(tokensEndpoint, factory, _serializer, Options));

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
            var sut = new AccountsEndpoint(new NordigenHttpClient(tokensEndpoint, factory, _serializer, Options));
            var result = await sut.Get(id, CancellationToken.None);

            result
                .Value.Should().BeOfType<Error>()
                .And.Subject.As<Error>()
                .StatusCode.Should().Be(500);
        }

        [Fact]
        public async Task Balances_When_Account_Is_Found_Returns_Valid_Information()
        {
            var id = Guid.NewGuid();
            var authToken = Guid.NewGuid().ToString();
            var handlerMock = new MockHttpMessageHandler();
            const string payload = "[{\"balanceAmount\": {\"amount\": \"-88.91\",\"currency\": \"string\"},\"balanceType\": \"string\",\"referenceDate\": \"2022-01-20\"},{\"balanceAmount\": {\"amount\": \"-119.23\",\"currency\": \"string\"},\"balanceType\": \"string\",\"referenceDate\": \"2022-01-23\"}]";
            var response = $"{{\"balances\":{payload}}}";
            handlerMock
                .When($"/api/v2/accounts/{id}/balances/")
                .With(x => x.Headers.Any(h => h.Key == "Authorization" && h.Value.First() == $"Bearer {authToken}"))
                .Respond("application/json", response);

            var client = new HttpClient(handlerMock) { BaseAddress = new Uri(Options.Url) };
            var tokensEndpoint = Mock.Of<ITokensEndpoint>();
            Mock.Get(tokensEndpoint)
                .Setup(x => x.Get(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Token(authToken, 10, string.Empty, 10));
            var factory = Mock.Of<IHttpClientFactory>();
            Mock.Get(factory).Setup(x => x.CreateClient("api")).Returns(client);
            var serializer = _serializer;
            var sut = new AccountsEndpoint(new NordigenHttpClient(tokensEndpoint, factory, serializer, Options));

            var result = await sut.Balances(id, CancellationToken.None);

            var expected = serializer.Deserialize<Balance[]>(payload);
            result.Value.Should().BeOfType<Balance[]>();
            result.Value.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task Details_When_Account_Is_Found_Returns_Valid_Information()
        {
            var id = Guid.NewGuid();
            var authToken = Guid.NewGuid().ToString();
            var handlerMock = new MockHttpMessageHandler();
            const string payload = "{\"resourceId\":\"string\",\"iban\":\"string\",\"currency\":\"string\",\"ownerName\":\"string\",\"name\":\"string\",\"product\":\"string\",\"cashAccountType\":\"string\"}";
            var response = $"{{\"account\":{payload}}}";
            handlerMock
                .When($"/api/v2/accounts/{id}/details/")
                .With(x => x.Headers.Any(h => h.Key == "Authorization" && h.Value.First() == $"Bearer {authToken}"))
                .Respond("application/json", response);

            var client = new HttpClient(handlerMock) { BaseAddress = new Uri(Options.Url) };
            var tokensEndpoint = Mock.Of<ITokensEndpoint>();
            Mock.Get(tokensEndpoint)
                .Setup(x => x.Get(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Token(authToken, 10, string.Empty, 10));
            var factory = Mock.Of<IHttpClientFactory>();
            Mock.Get(factory).Setup(x => x.CreateClient("api")).Returns(client);
            var serializer = _serializer;
            var sut = new AccountsEndpoint(new NordigenHttpClient(tokensEndpoint, factory, serializer, Options));

            var result = await sut.Details(id, CancellationToken.None);

            var expected = serializer.Deserialize<AccountDetails>(payload);
            result.Value.Should().BeOfType<AccountDetails>();
            result.Value.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task Transactions_When_Account_Is_Found_Returns_Valid_Information()
        {
            var from = DateTime.Now.Date;
            var to = DateTime.Now.Date.AddDays(1);
            var id = Guid.NewGuid();
            var authToken = Guid.NewGuid().ToString();
            var handlerMock = new MockHttpMessageHandler();
            const string payload = "{\"booked\": [{\"transactionId\": \"string\",\"debtorName\": \"string\",\"debtorAccount\": {\"iban\": \"string\"},\"transactionAmount\": {\"currency\": \"string\",\"amount\": \"855.51\"},\"bankTransactionCode\": \"string\",\"bookingDate\": \"2022-01-23\",\"valueDate\": \"2022-01-19\",\"remittanceInformationUnstructured\": \"string\"},{\"transactionId\": \"string\",\"transactionAmount\": {\"currency\": \"string\",\"amount\": \"-891.75\"},\"bankTransactionCode\": \"string\",\"bookingDate\": \"2022-01-23\",\"valueDate\": \"2022-01-21\",\"remittanceInformationUnstructured\": \"string\"}],\"pending\": [{\"transactionAmount\": {\"currency\": \"string\",\"amount\": \"423.35\"},\"valueDate\": \"2022-01-18\",\"remittanceInformationUnstructured\": \"string\"}]}";
            var response = $"{{\"transactions\":{payload}}}";
            handlerMock
                .When($"/api/v2/accounts/{id}/transactions/?date_from={from:yyyy-MM-dd}&date_to={to:yyyy-MM-dd}")
                .With(x => x.Headers.Any(h => h.Key == "Authorization" && h.Value.First() == $"Bearer {authToken}"))
                .Respond("application/json", response);

            var client = new HttpClient(handlerMock) { BaseAddress = new Uri(Options.Url) };
            var tokensEndpoint = Mock.Of<ITokensEndpoint>();
            Mock.Get(tokensEndpoint)
                .Setup(x => x.Get(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Token(authToken, 10, string.Empty, 10));
            var factory = Mock.Of<IHttpClientFactory>();
            Mock.Get(factory).Setup(x => x.CreateClient("api")).Returns(client);
            var serializer = _serializer;
            var sut = new AccountsEndpoint(new NordigenHttpClient(tokensEndpoint, factory, serializer, Options));
            var filter = new AccountTransactionsFilter(from, to);
            var result = await sut.Transactions(id, filter, CancellationToken.None);

            var expected = serializer.Deserialize<Transactions>(payload);
            result.Value.Should().BeOfType<Transactions>();
            result.Value.Should().BeEquivalentTo(expected);
        }
    }
}
