using System;
using System.Threading;
using System.Threading.Tasks;
using Nordigen.Net.Queries;
using Nordigen.Net.Responses;
using Agreement = Nordigen.Net.Requests.Agreement;

namespace Nordigen.Net.Internal;

internal class AgreementsEndpoint : IAgreementsEndpoint, IEndpoint
{
    private readonly INordigenHttpClient _client;

    internal AgreementsEndpoint(INordigenHttpClient client)
    {
        _client = client;
    }

    public async Task<NOneOf<Responses.Agreement, Error>> Post(Agreement request, CancellationToken cancellationToken)
    {
        return await _client.Post<Agreement, Responses.Agreement>(request, "/api/v2/agreements/enduser/", cancellationToken);
    }

    public Task<NOneOf<Responses.Agreement, Error>> Get(Guid id, CancellationToken cancellationToken = default)
        => _client.Get<Responses.Agreement>($"api/v2/agreements/enduser/{id}/", cancellationToken);

    public async Task<NOneOf<PaginationResult<Responses.Agreement>, Error>> Paginate(Paginate<Responses.Agreement> command, CancellationToken cancellationToken = default)
    {
        var result = await _client.Get<Internal.Model.PaginationResult<Responses.Agreement>>($"api/v2/agreements/enduser/?limit={command.Limit}&offset={command.Offset}", cancellationToken);
        return result.Match(x => NOneOf<PaginationResult<Responses.Agreement>, Error>.FromT0(new PaginationResult<Responses.Agreement>(x, command.Limit, command.Offset)), _ => _);
    }

    public Task<NOneOf<Deleted, Error>> Delete(Guid id, CancellationToken cancellationToken = default)
    {
        return _client.Delete($"api/v2/agreements/enduser/{id}/", cancellationToken);
    }
}
