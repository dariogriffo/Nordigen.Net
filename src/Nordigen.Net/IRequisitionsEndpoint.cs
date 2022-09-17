namespace Nordigen.Net;

using Queries;
using Responses;
using System;
using System.Threading;
using System.Threading.Tasks;

public interface IRequisitionsEndpoint
{
    /// <summary>
    /// Get details about a specific requisition.
    /// </summary>
    /// <param name="id">The id of the requisition</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/></param>
    /// <returns>A list of <see cref="Institution"/> or <see cref="Error"/></returns>
    Task<NOneOf<Requisition, Error>> Get(string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Paginate requisitions.
    /// </summary>
    /// <param name="command">The Pagination command</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/></param>
    /// <returns>A list of <see cref="Institution"/> or <see cref="Error"/></returns>
    Task<NOneOf<PaginationResult<Requisition>, Error>> Paginate(Paginate<Requisition> command, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new requisition
    /// </summary>
    /// <param name="requisition"> The <see cref="Requisition"/></param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/></param>
    /// <returns></returns>
    Task<NOneOf<Requisition, Error>> Post(Requests.Requisition requisition, CancellationToken cancellationToken = default);


    /// <summary>
    /// Deletes a requisition.
    /// </summary>
    /// <param name="id">The id of the requisition</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/></param>
    /// <returns><see cref="Deleted"/> or <see cref="Error"/></returns>
    Task<NOneOf<Deleted, Error>> Delete(string id, CancellationToken cancellationToken = default);

}