using System;
using Microsoft.Extensions.DependencyInjection;
using Nordigen.Net.Internal;
using ServiceCollection.Extensions.Modules;

namespace Nordigen.Net
{
    public class NordigenApiModule : Module
    {
        private readonly NordigenApiOptions _options;

        public NordigenApiModule(NordigenApiOptions options)
        {
            _options = options;
        }

        protected override void Load(IServiceCollection services)
        {
            services.AddSingleton(_options);
            services.AddSingleton<ITokensEndpoint, TokensEndpoint>();
            services.AddHttpClient<TokensEndpoint>("tokens", x =>
             {
                 x.DefaultRequestHeaders.Add("Accept", "application/json");
                 x.BaseAddress = new Uri($"{_options.Url.TrimEnd('/')}");
             });

            services.AddSingleton<INordigenHttpClient, NordigenHttpClient>();
            services.AddHttpClient<NordigenHttpClient>("api", x =>
            {
                x.DefaultRequestHeaders.Add("Accept", "application/json");
                x.BaseAddress = new Uri($"{_options.Url.TrimEnd('/')}");
            });
            services.AddSingleton<INordigenApi, NordigenApi>();
            services.AddSingleton<IAccountsEndpoint, AccountsEndpoint>();
            services.AddSingleton<IInstitutionsEndpoint, InstitutionsEndpoint>();
        }
    }
}
