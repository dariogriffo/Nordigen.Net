namespace Nordigen.Net.Responses;

using Queries;

public class PaginationResult<T>
{
    internal PaginationResult(
        Internal.Model.PaginationResult<T> paginationResult,
        int limit,
        int offset)
    {
        Result = paginationResult.Results;
        var taken = offset + Result.Length;
        Next = taken != paginationResult.Count
            ? (NOneOf<Paginate<T>, End>)new Paginate<T>(limit, taken)
            : End.Value;
    }

    public T[] Result { get; }

    public NOneOf<Paginate<T>, End> Next { get; }
}