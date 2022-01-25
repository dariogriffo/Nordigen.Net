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

namespace Nordigen.Net.UnitTests
{
    public class InstitutionsEndpointTests
    {
        private static readonly NordigenApiOptions Options = new NordigenApiOptions()
        {
            Url = "https://api.nordigen.com/"
        };

        [Fact]
        public async Task Get_When_Country_Is_Found_Returns_Valid_Information()
        {
            const string id = "GB";
            var authToken = Guid.NewGuid().ToString();
            var handlerMock = new MockHttpMessageHandler();
            handlerMock
                .When($"/api/v2/institutions/?country={id}")
                .With(x => x.Headers.Any(h => h.Key == "Authorization" && h.Value.First() == $"Bearer {authToken}"))
                .Respond("application/json",
                    "[{\"id\": \"ABNAMRO_ABNAGB2LXXX\",\"name\": \"ABN AMRO Bank Commercial\",\"bic\": \"ABNAGB2LXXX\",\"transaction_total_days\": \"540\",\"countries\": [\"GB\"],\"logo\": \"https://cdn.nordigen.com/ais/ABNAMRO_FTSBDEFAXXX.png\"},{\"id\": \"AMERICAN_EXPRESS_AESUGB21\",\"name\": \"American Express\",\"bic\": \"AESUGB21\",\"transaction_total_days\": \"90\",\"countries\": [\"GB\",\"FR\",\"FI\",\"SE\",\"GB\"],\"logo\": \"https://cdn.nordigen.com/ais/AMERICAN_EXPRESS_AESUGB21.png\"}]"
                );

            var client = new HttpClient(handlerMock) { BaseAddress = new Uri(Options.Url) };
            var tokensEndpoint = Mock.Of<ITokensEndpoint>();
            Mock.Get(tokensEndpoint)
                .Setup(x => x.Get(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Token(authToken, 10, string.Empty, 10));
            var factory = Mock.Of<IHttpClientFactory>();
            Mock.Get(factory).Setup(x => x.CreateClient("api")).Returns(client);
            var sut = new InstitutionsEndpoint(new NordigenHttpClient(tokensEndpoint, factory, Options));

            var result = await sut.GetByCountryIso3166Code(id, CancellationToken.None);

            var insitution1 = new Institution(
                "ABNAMRO_ABNAGB2LXXX",
                "ABN AMRO Bank Commercial",
                "ABNAGB2LXXX",
                "540",
                new[] { "GB" },
                "https://cdn.nordigen.com/ais/ABNAMRO_FTSBDEFAXXX.png");

            var insitution2 = new Institution(
                "AMERICAN_EXPRESS_AESUGB21",
                "American Express",
                "AESUGB21",
                "90",
                new[] { "GB", "FR", "FI", "SE" },
                "https://cdn.nordigen.com/ais/AMERICAN_EXPRESS_AESUGB21.png");

            var expected = new List<Institution>() { insitution1, insitution2 };
            result.Value.Should().BeEquivalentTo(expected);
        }
    }
}
