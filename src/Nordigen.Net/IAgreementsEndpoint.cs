using System;
using System.Threading;
using System.Threading.Tasks;
using Nordigen.Net.Queries;
using Nordigen.Net.Responses;

namespace Nordigen.Net;

public interface IAgreementsEndpoint
{
    /// <summary>
    /// Creates enduser agreement
    /// </summary>
    /// <param name="request">The <see cref="Requests.Agreement"/></param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/></param>
    /// <returns>The <see cref="Responses.Agreement"/> or <see cref="Error"/></returns>
    public Task<NOneOf<Agreement, Error>> Post(Requests.Agreement request, CancellationToken cancellationToken);
    
    /// <summary>
    /// Get details about a specific agreement.
    /// </summary>
    /// <param name="id">The id of the agreement</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/></param>
    /// <returns>The <see cref="Responses.Agreement"/> or <see cref="Error"/></returns>
    /// <returns></returns>
    Task<NOneOf<Agreement, Error>> Get(Guid id, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Paginate agreements.
    /// </summary>
    /// <param name="command">The Pagination command</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/></param>
    /// <returns>A list of <see cref="Responses.Agreement"/> or <see cref="Error"/></returns>
    Task<NOneOf<PaginationResult<Agreement>, Error>> Paginate(Paginate<Agreement> command, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an agreement.
    /// </summary>
    /// <param name="id">The id of the requisition</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/></param>
    /// <returns><see cref="Deleted"/> or <see cref="Error"/></returns>
    public Task<NOneOf<Deleted, Error>> Delete(Guid id, CancellationToken cancellationToken = default);
}
