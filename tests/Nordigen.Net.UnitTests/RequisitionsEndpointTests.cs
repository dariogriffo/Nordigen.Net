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
using Nordigen.Net.Queries;
using Nordigen.Net.Responses;
using RichardSzalay.MockHttp;
using Xunit;

namespace Nordigen.Net.UnitTests;

public class RequisitionsEndpointTests
{
    private static readonly NordigenApiOptions Options = new NordigenApiOptions();
    private readonly ISerializer _serializer = new Serializer();

    [Fact]
    public async Task Post_When_Request_Is_Valid_Returns_Requisition()
    {
        var id = Guid.NewGuid();
        var authToken = Guid.NewGuid().ToString();
        var handlerMock = new MockHttpMessageHandler();
        const string payload =
            "{\"id\":\"3fa85f64-5717-4562-b3fc-2c963f66afa6\",\"created\":\"2022-02-19T13:00:48.495Z\",\"redirect\":\"string\",\"status\":\"CR\",\"institution_id\":\"3fa85f64-5717-4562-b3fc-2c963f66afa6\",\"agreement\":\"3fa85f64-5717-4562-b3fc-2c963f66afa6\",\"reference\":\"string\",\"accounts\":[\"3fa85f64-5717-4562-b3fc-2c963f66afa6\"],\"user_language\":\"string\",\"link\":\"https://ob.nordigen.com/psd2/start/3fa85f64-5717-4562-b3fc-2c963f66afa6/{$INSTITUTION_ID}\",\"ssn\":\"string\",\"account_selection\":false}";
        handlerMock
            .When(HttpMethod.Post, $"/api/v2/requisitions/")
            .With(x => x.Headers.Any(h => h.Key == "Authorization" && h.Value.First() == $"Bearer {authToken}"))
            .Respond("application/json", payload);

        var expected = _serializer.Deserialize<Responses.Requisition>(payload);

        var client = new HttpClient(handlerMock) { BaseAddress = new Uri(Options.Url) };
        var tokensEndpoint = Mock.Of<ITokensEndpoint>();
        Mock.Get(tokensEndpoint)
            .Setup(x => x.Get(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Token(authToken, 10, string.Empty, 10));
        var factory = Mock.Of<IHttpClientFactory>();
        Mock.Get(factory).Setup(x => x.CreateClient("api")).Returns(client);

        var request = new Requests.Requisition(
            "redirect",
            Guid.NewGuid().ToString(), 
            Guid.NewGuid().ToString(), 
            "reference",
            "userLanguage",
            "ssn",
            true);

        var sut = new RequisitionsEndpoint(new NordigenHttpClient(tokensEndpoint, factory, _serializer, Options));

        var result = await sut.Post(request, CancellationToken.None);
        result.AsT0.Should().BeEquivalentTo(expected, x => x.Excluding(y => y.Created));
    }

    [Fact]
    public async Task Paginate_When_Requisitions_Are_Found_Returns_Valid_List()
    {
        var command = new Paginate<Requisition>(1, 10);
        var id = Guid.NewGuid();
        var authToken = Guid.NewGuid().ToString();
        var handlerMock = new MockHttpMessageHandler();
        const string payload =
            "{\"count\":123,\"next\":\"https://ob.nordigen.com/api/v2/requisitions/?limit=100&offset=0\",\"previous\":\"https://ob.nordigen.com/api/v2/requisitions/?limit=100&offset=0\",\"results\":[{\"id\":\"3fa85f64-5717-4562-b3fc-2c963f66afa6\",\"created\":\"2022-02-19T13:02:24.471Z\",\"redirect\":\"string\",\"status\":\"CR\",\"institution_id\":\"3fa85f64-5717-4562-b3fc-2c963f66afa6\",\"agreement\":\"3fa85f64-5717-4562-b3fc-2c963f66afa6\",\"reference\":\"string\",\"accounts\":[\"3fa85f64-5717-4562-b3fc-2c963f66afa6\"],\"user_language\":\"string\",\"link\":\"https://ob.nordigen.com/psd2/start/3fa85f64-5717-4562-b3fc-2c963f66afa6/{$INSTITUTION_ID}\",\"ssn\":\"string\",\"account_selection\":false}]}";
        handlerMock
            .When(HttpMethod.Get, $"/api/v2/requisitions/?limit={command.Limit}&offset={command.Offset}")
            .With(x => x.Headers.Any(h => h.Key == "Authorization" && h.Value.First() == $"Bearer {authToken}"))
            .Respond("application/json", payload);

        var internalResult = _serializer.Deserialize<Internal.Model.PaginationResult<Responses.Requisition>>(payload);
        var expected = new PaginationResult<Responses.Requisition>(internalResult, command.Limit, command.Offset);

        var client = new HttpClient(handlerMock) { BaseAddress = new Uri(Options.Url) };
        var tokensEndpoint = Mock.Of<ITokensEndpoint>();
        Mock.Get(tokensEndpoint)
            .Setup(x => x.Get(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Token(authToken, 10, string.Empty, 10));

        var factory = Mock.Of<IHttpClientFactory>();
        Mock.Get(factory).Setup(x => x.CreateClient("api")).Returns(client);

        var sut = new RequisitionsEndpoint(new NordigenHttpClient(tokensEndpoint, factory, _serializer, Options));
        var result = await sut.Paginate(command, CancellationToken.None);
        result.AsT0.Result.Should().BeEquivalentTo(expected.Result);
        result.AsT0.Next.AsT0.Should().BeEquivalentTo(expected.Next.AsT0);
    }

    [Fact]
    public async Task Get_Single_Requisition_When_Requisition_Is_Found_Returns_Valid_Information()
    {
        var id = Guid.NewGuid();
        var authToken = Guid.NewGuid().ToString();
        var handlerMock = new MockHttpMessageHandler();
        const string payload =
            "{\"id\":\"3fa85f64-5717-4562-b3fc-2c963f66afa6\",\"created\":\"2022-02-19T13:00:48.495Z\",\"redirect\":\"string\",\"status\":\"CR\",\"institution_id\":\"3fa85f64-5717-4562-b3fc-2c963f66afa6\",\"agreement\":\"3fa85f64-5717-4562-b3fc-2c963f66afa6\",\"reference\":\"string\",\"accounts\":[\"3fa85f64-5717-4562-b3fc-2c963f66afa6\"],\"user_language\":\"string\",\"link\":\"https://ob.nordigen.com/psd2/start/3fa85f64-5717-4562-b3fc-2c963f66afa6/{$INSTITUTION_ID}\",\"ssn\":\"string\",\"account_selection\":false}";
        handlerMock
            .When(HttpMethod.Get, $"/api/v2/requisitions/{id}/")
            .With(x => x.Headers.Any(h => h.Key == "Authorization" && h.Value.First() == $"Bearer {authToken}"))
            .Respond("application/json", payload);

        var expected = _serializer.Deserialize<Responses.Requisition>(payload);

        var client = new HttpClient(handlerMock) { BaseAddress = new Uri(Options.Url) };
        var tokensEndpoint = Mock.Of<ITokensEndpoint>();
        Mock.Get(tokensEndpoint)
            .Setup(x => x.Get(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Token(authToken, 10, string.Empty, 10));
        var factory = Mock.Of<IHttpClientFactory>();
        Mock.Get(factory).Setup(x => x.CreateClient("api")).Returns(client);

        var sut = new RequisitionsEndpoint(new NordigenHttpClient(tokensEndpoint, factory, _serializer, Options));

        var result = await sut.Get(id, CancellationToken.None);
        result.AsT0.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task Get_Single_Requisition_When_Requisition_Is_Not_Found_Returns_NotFound()
    {
        var id = Guid.NewGuid();
        var authToken = Guid.NewGuid().ToString();
        var handlerMock = new MockHttpMessageHandler();
        handlerMock
            .When(HttpMethod.Get, $"/api/v2/requisitions/{id}/")
            .With(x => x.Headers.Any(h => h.Key == "Authorization" && h.Value.First() == $"Bearer {authToken}"))
            .Respond(_ => new HttpResponseMessage(HttpStatusCode.NotFound)
            {
                Content = new ReadOnlyMemoryContent(
                    Encoding.UTF8.GetBytes("{\"status_code\": 404,\"summary\":\"Invalid Responses.Requisition ID\"}"))
            });

        var client = new HttpClient(handlerMock) { BaseAddress = new Uri(Options.Url) };
        var tokensEndpoint = Mock.Of<ITokensEndpoint>();
        Mock.Get(tokensEndpoint)
            .Setup(x => x.Get(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Token(authToken, 10, string.Empty, 10));
        var factory = Mock.Of<IHttpClientFactory>();
        Mock.Get(factory).Setup(x => x.CreateClient("api")).Returns(client);
        var sut = new RequisitionsEndpoint(new NordigenHttpClient(tokensEndpoint, factory, _serializer, Options));

        var result = await sut.Get(id, CancellationToken.None);

        result.AsT1.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task Delete_When_Requisition_Is_Found_Returns_Deleted()
    {
        var id = Guid.NewGuid();
        var authToken = Guid.NewGuid().ToString();
        var handlerMock = new MockHttpMessageHandler();
        const string payload =
            "{\"summary\":\"EndUser Requisition deleted\",\"detail\":\"EndUser Requisition '$EUA_ID' deleted\"}";
        handlerMock
            .When(HttpMethod.Delete, $"/api/v2/requisitions/{id}/")
            .With(x => x.Headers.Any(h => h.Key == "Authorization" && h.Value.First() == $"Bearer {authToken}"))
            .Respond("application/json", payload);
        
        var expected = _serializer.Deserialize<Responses.Deleted>(payload);

        var client = new HttpClient(handlerMock) { BaseAddress = new Uri(Options.Url) };
        var tokensEndpoint = Mock.Of<ITokensEndpoint>();
        Mock.Get(tokensEndpoint)
            .Setup(x => x.Get(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Token(authToken, 10, string.Empty, 10));
        var factory = Mock.Of<IHttpClientFactory>();
        Mock.Get(factory).Setup(x => x.CreateClient("api")).Returns(client);

        var sut = new RequisitionsEndpoint(new NordigenHttpClient(tokensEndpoint, factory, _serializer, Options));

        var result = await sut.Delete(id, CancellationToken.None);
        result.AsT0.Should().BeEquivalentTo(expected);
    }
}
