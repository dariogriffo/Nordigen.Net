using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OneOf;

namespace Nordigen.Net.Internal
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using Responses;

    internal class TokensEndpoint : ITokensEndpoint
    {
        private static readonly JsonSerializerSettings Settings = new JsonSerializerSettings()
        {
            ContractResolver =  new CamelCasePropertyNamesContractResolver()
        };

        private readonly HttpClient _client;
        private readonly NordigenApiOptions _options;

        public TokensEndpoint(IHttpClientFactory factory, NordigenApiOptions options)
        {
            _client = factory.CreateClient("tokens");
            _options = options;
        }

        public async Task<OneOf<Token, Error>> Get(CancellationToken cancellationToken)
        {
            var credentials = new
            {
                _options.SecretId,
                _options.SecretKey
            };

            var content = new StringContent(JsonConvert.SerializeObject(credentials, Settings), Encoding.UTF8, "application/json");
            var message = await _client.PostAsync("api/v2/token/new/", content, cancellationToken);
            if (message.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<Token>(await message.Content.ReadAsStringAsync(), Settings);
            }

            return JsonConvert.DeserializeObject<Error>(await message.Content.ReadAsStringAsync(), Settings);
        }

        public async Task<OneOf<Token, Error>> Refresh(string refresh, CancellationToken cancellationToken = default)
        {
            var credentials = new
            {
                refresh
            };

            var content = new StringContent(JsonConvert.SerializeObject(credentials, Settings), Encoding.UTF8, "application/json");

            var message = await _client.PostAsync("api/v2/token/refresh/", content, cancellationToken);
            if (message.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<Token>(await message.Content.ReadAsStringAsync(), Settings);
            }

            return JsonConvert.DeserializeObject<Error>(await message.Content.ReadAsStringAsync(), Settings);
        }
    }
}
