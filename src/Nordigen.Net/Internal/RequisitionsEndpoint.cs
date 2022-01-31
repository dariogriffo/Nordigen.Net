namespace Nordigen.Net.Internal
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Queries;
    using Responses;

    public class RequisitionsEndpoint : IRequisitionsEndpoint
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

        public async Task<NOneOf<Responses.PaginationResult<Requisition>, Error>> Paginate(Paginate<Requisition> command, CancellationToken cancellationToken = default)
        {
            var result = await _client.Get<Internal.Model.PaginationResult<Requisition>>($"api/v2/requisitions/?limit={command.Limit}&offset={command.Offset}", cancellationToken);
            return result.Match(x => NOneOf<Responses.PaginationResult<Requisition>, Error>.FromT0(new Responses.PaginationResult<Requisition>(x, command.Limit, command.Offset)), _ => _);
        }

        public async Task<NOneOf<Requisition, Error>> Post(Requisition requisition, CancellationToken cancellationToken = default)
        {
            var model = new Internal.Model.RequisitionPost(requisition.Redirect, requisition.InstitutionId, requisition.Agreement, requisition.Reference, requisition.UserLanguage, requisition.Ssn, requisition.AccountSelection);
            return await _client.Post<Internal.Model.RequisitionPost, Requisition>(model, "api/v2/requisitions/", cancellationToken);
        }

        public Task<NOneOf<Deleted, Error>> Delete(Guid id, CancellationToken cancellationToken = default)
        {
            return _client.Delete($"api/v2/requisitions/{id}/", cancellationToken);
        }
    }
}
