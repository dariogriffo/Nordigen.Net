namespace Nordigen.Net.Internal;

using Queries;
using Responses;
using System;
using System.Threading;
using System.Threading.Tasks;

public class RequisitionsEndpoint : IRequisitionsEndpoint, IEndpoint
{
    private readonly INordigenHttpClient _client;

    internal RequisitionsEndpoint(INordigenHttpClient client)
    {
        _client = client;
    }

    public Task<NOneOf<Requisition, Error>> Get(Guid id, CancellationToken cancellationToken = default)
    {
        return _client.Get<Requisition>($"api/v2/requisitions/{id}/", cancellationToken);
    }

    public async Task<NOneOf<PaginationResult<Requisition>, Error>> Paginate(Paginate<Requisition> command, CancellationToken cancellationToken = default)
    {
        var result = await _client.Get<Internal.Model.PaginationResult<Requisition>>($"api/v2/requisitions/?limit={command.Limit}&offset={command.Offset}", cancellationToken);
        return result.Match(x => NOneOf<PaginationResult<Requisition>, Error>.FromT0(new PaginationResult<Requisition>(x, command.Limit, command.Offset)), _ => _);
    }

    public async Task<NOneOf<Requisition, Error>> Post(Requests.Requisition request, CancellationToken cancellationToken = default)
    {
        return await _client.Post<Requests.Requisition, Requisition>(request, "api/v2/requisitions/", cancellationToken);
    }

    public Task<NOneOf<Deleted, Error>> Delete(Guid id, CancellationToken cancellationToken = default)
    {
        return _client.Delete($"api/v2/requisitions/{id}/", cancellationToken);
    }
}