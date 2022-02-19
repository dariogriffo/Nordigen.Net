namespace Nordigen.Net;

using Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

public static class NordigenApiServiceCollectionExtensions
{
    /// <summary>
    /// Adds Nordigen.NET to the DI container
    /// </summary>
    /// <param name="services"></param>
    /// <returns> a<see cref="IHttpClientBuilder"/> to configure retries with <see cref="Polly"/></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IHttpClientBuilder AddNordigenApi(this IServiceCollection services)
    {
        services.AddSingleton(serviceProvider =>
        {
            var options = serviceProvider
                .GetRequiredService<IConfiguration>()
                .GetSection("NordigenApi").Get<NordigenApiOptions>();
            if (string.IsNullOrWhiteSpace(options.SecretId) || string.IsNullOrWhiteSpace(options.SecretKey))
            {
                throw new InvalidOperationException("SecretId and SecretKey are required to connect to Nordigen");
            }
            
            return options;
        });

        services.AddSingleton<ISerializer, Serializer>();
        services.AddSingleton<ITokensEndpoint, TokensEndpoint>();
        var assemblyName = typeof(INordigenApi).Assembly.GetName();
        var userAgentHeader = $"{assemblyName.Name}/{assemblyName.Version}";
        services.AddHttpClient(TokensEndpoint.HttpClientName, (serviceProvider, client) =>
        {
            var options = serviceProvider
                              .GetRequiredService<NordigenApiOptions>() ??
                          throw new ArgumentNullException(nameof(NordigenApiOptions));
            client.DefaultRequestHeaders.Add("Accept", Constants.AcceptedMediaType);
            client.DefaultRequestHeaders.Add("User-Agent", userAgentHeader);
            client.BaseAddress = new Uri($"{options.Url.TrimEnd('/')}");
        });

        services.AddSingleton<INordigenHttpClient, NordigenHttpClient>();
        services.AddSingleton<INordigenApi, NordigenApi>();
        services.AddSingleton<IAccountsEndpoint, AccountsEndpoint>();
        services.AddSingleton<IInstitutionsEndpoint, InstitutionsEndpoint>();
        services.AddSingleton<IRequisitionsEndpoint, RequisitionsEndpoint>();
        services.AddSingleton<IAgreementsEndpoint, AgreementsEndpoint>();
        return services.AddHttpClient(NordigenHttpClient.HttpClientName, (serviceProvider, client) =>
        {
            var options = serviceProvider
                              .GetRequiredService<NordigenApiOptions>() ??
                          throw new ArgumentNullException(nameof(NordigenApiOptions));
            client.DefaultRequestHeaders.Add("Accept", Constants.AcceptedMediaType);
            client.DefaultRequestHeaders.Add("User-Agent", userAgentHeader);
            client.BaseAddress = new Uri($"{options.Url.TrimEnd('/')}");
        });
    }
}
