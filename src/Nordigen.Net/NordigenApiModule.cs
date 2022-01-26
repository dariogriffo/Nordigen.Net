namespace Nordigen.Net
{
    using System;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Nordigen.Net.Internal;
    using ServiceCollection.Extensions.Modules;

    public class NordigenApiModule : Module
    {
        protected override void Load(IServiceCollection services)
        {
            services.AddSingleton(serviceProvider =>
            {
                return serviceProvider
                        .GetRequiredService<IConfiguration>()
                        .GetSection("NordigenApi").Get<NordigenApiOptions>() ??
                            throw new ArgumentNullException(nameof(NordigenApiOptions));
            });

            services.AddSingleton<ISerializer, Serializer>();
            services.AddSingleton<ITokensEndpoint, TokensEndpoint>();
            services.AddHttpClient(TokensEndpoint.HttpClientName, (serviceProvider, client) =>
            {
                var options = serviceProvider
                                  .GetRequiredService<NordigenApiOptions>() ??
                              throw new ArgumentNullException(nameof(NordigenApiOptions));
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.BaseAddress = new Uri($"{options.Url.TrimEnd('/')}");
            });

            services.AddSingleton<INordigenHttpClient, NordigenHttpClient>();
            services.AddHttpClient(NordigenHttpClient.HttpClientName, (serviceProvider, client) =>
            {
                var options = serviceProvider
                                  .GetRequiredService<NordigenApiOptions>() ??
                              throw new ArgumentNullException(nameof(NordigenApiOptions));
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.BaseAddress = new Uri($"{options.Url.TrimEnd('/')}");
            });
            services.AddSingleton<INordigenApi, NordigenApi>();
            services.AddSingleton<IAccountsEndpoint, AccountsEndpoint>();
            services.AddSingleton<IInstitutionsEndpoint, InstitutionsEndpoint>();
        }
    }
}
