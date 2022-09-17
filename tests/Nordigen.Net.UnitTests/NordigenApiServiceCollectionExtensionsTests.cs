using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Nordigen.Net.Internal;
using Xunit;

namespace Nordigen.Net.UnitTests;

public class NordigenApiServiceCollectionExtensionsTests
{
    [Theory]
    [InlineData(typeof(INordigenApi), typeof(NordigenApi))]
    [InlineData(typeof(IAccountsEndpoint), typeof(AccountsEndpoint))]
    [InlineData(typeof(IAgreementsEndpoint), typeof(AgreementsEndpoint))]
    [InlineData(typeof(IRequisitionsEndpoint), typeof(RequisitionsEndpoint))]
    [InlineData(typeof(ITokensEndpoint), typeof(TokensEndpoint))]
    [InlineData(typeof(IInstitutionsEndpoint), typeof(InstitutionsEndpoint))]
    [InlineData(typeof(ISerializer), typeof(Serializer))]
    [InlineData(typeof(INordigenHttpClient), typeof(NordigenHttpClient))]
    public void AddNordigenApi_Ensure_All_Requirements_Are_Registered_Correctly(Type service, Type implementation)
    {
        var services = new ServiceCollection();
        services.AddNordigenApi();
        services
            .Any(x => x.ServiceType == service && x.ImplementationType == implementation && x.Lifetime == ServiceLifetime.Singleton)
            .Should().BeTrue();
    }

    [Fact]
    public void AddNordigenApi_Ensure_All_Requirements_Are_Registered()
    {
        var services = new ServiceCollection();
        services.AddNordigenApi();
        services.Count.Should().Be(27);
    }

    [Fact]
    public void AddNordigenApi_Ensure_HttpClient_Is_Registered_With_Correct_Name()
    {
        var services = new ServiceCollection();
        var builder = services.AddNordigenApi();
        builder.Name.Should().Be(NordigenHttpClient.HttpClientName);
    }

    [Fact]
    public void AddNordigenApi_Ensure_HttpClient_Is_Registered_With_Correct_Headers()
    {
        var services = new ServiceCollection();
        services.AddNordigenApi();
        var dictionary = new Dictionary<string, string>()
        {
            { "NordigenApi:SecretId", "test" },
            { "NordigenApi:SecretKey", "test" },
        };
        var configuration = new ConfigurationBuilder().AddInMemoryCollection(dictionary).Build();
        services.AddSingleton<IConfiguration>(configuration);
        using var provider = services.BuildServiceProvider();

        var client = provider.GetRequiredService<IHttpClientFactory>().CreateClient(NordigenHttpClient.HttpClientName);
        var result = client.DefaultRequestHeaders.UserAgent.First().Product!;
        result.Name.Should().Be(typeof(INordigenApi).Assembly.GetName().Name!);
        result.Version.Should().Be(typeof(INordigenApi).Assembly.GetName().Version!.ToString());

        client.DefaultRequestHeaders.Accept.First().MediaType!.Should().Be(Constants.AcceptedMediaType);
    }

    [Fact]
    public void AddNordigenApi_When_Configuration_Is_Not_Provided_Throws_Exception()
    {
        var services = new ServiceCollection();
        services.AddNordigenApi();
        using var provider = services.BuildServiceProvider();
        // ReSharper disable once AccessToDisposedClosure
        Action act = () => provider.GetService<INordigenApi>();
        act.Should().Throw<InvalidOperationException>();
    }

    [Theory]
    [InlineData(null, null)]
    [InlineData(null, "")]
    [InlineData("", "")]
    [InlineData("a", "")]
    [InlineData("", "a")]
    [InlineData(null, "a")]
    [InlineData("a", null)]
    public void AddNordigenApi_With_Invalid_Options_Throws_Exception(string secretId, string secretKey)
    {
        var services = new ServiceCollection();
        services.AddNordigenApi();
        var dictionary = new Dictionary<string, string>()
        {
            { "NordigenApi:SecretId", secretId },
            { "NordigenApi:SecretKey", secretKey },
        };
        var configuration = new ConfigurationBuilder().AddInMemoryCollection(dictionary).Build();
        services.AddSingleton<IConfiguration>(configuration);
        using var provider = services.BuildServiceProvider();
        // ReSharper disable once AccessToDisposedClosure
        Action act = () => provider.GetService<INordigenApi>();
        act.Should().Throw<InvalidOperationException>();
    }
    
    [Fact]
    public void AddNordigenApi_With_Valid_Api_Is_Resolved_Correctly()
    {
        var services = new ServiceCollection();
        services.AddNordigenApi();
        var dictionary = new Dictionary<string, string>()
        {
            { "NordigenApi:SecretId", "secretId" },
            { "NordigenApi:SecretKey", "secretKey" },
        };
        var configuration = new ConfigurationBuilder().AddInMemoryCollection(dictionary).Build();
        services.AddSingleton<IConfiguration>(configuration);
        using var provider = services.BuildServiceProvider();
        // ReSharper disable once AccessToDisposedClosure
        var api = provider.GetService<INordigenApi>();
        api.Should().NotBeNull();
    }
    
    [Theory]
    [InlineData(typeof(IAccountsEndpoint))]
    [InlineData(typeof(IAgreementsEndpoint))]
    [InlineData(typeof(IRequisitionsEndpoint))]
    [InlineData(typeof(IInstitutionsEndpoint))]
    [InlineData(typeof(IAgreementsEndpoint))]
    void AddNordigenApi_With_Valid_All_Endpoints_Are_Resolved_Correctly(Type type)
    {
        var services = new ServiceCollection();
        services.AddNordigenApi();
        var dictionary = new Dictionary<string, string>()
        {
            { "NordigenApi:SecretId", "secretId" },
            { "NordigenApi:SecretKey", "secretKey" },
        };
        var configuration = new ConfigurationBuilder().AddInMemoryCollection(dictionary).Build();
        services.AddSingleton<IConfiguration>(configuration);
        using var provider = services.BuildServiceProvider();
        // ReSharper disable once AccessToDisposedClosure
        var implementation = provider.GetService(type);
        implementation.Should().NotBeNull();
    }
}
