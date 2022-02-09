namespace Nordigen.Net.Internal.Model
{
    using Newtonsoft.Json;
    using System.Collections.Generic;


    public class PaginationResult<T>
    {
        [JsonConstructor]
        public PaginationResult(
            int count,
            string next,
            string previous,
            T[] results
        )
        {
            Count = count;
            Next = next;
            Previous = previous;
            Results = results;
        }

        public int Count { get; }

        public string Next { get; }

        public string Previous { get; }

        public T[] Results { get; }
    }
}
