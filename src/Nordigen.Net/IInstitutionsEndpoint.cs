using System.Threading;
using System.Threading.Tasks;
using Nordigen.Net.Responses;
using OneOf;

namespace Nordigen.Net
{
    public interface IInstitutionsEndpoint
    {
        Task<OneOf<Institution[], Error>> GetByCountryIso3166Code(string id, CancellationToken cancellationToken = default);
    }
}
