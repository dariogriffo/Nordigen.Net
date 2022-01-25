using System;
using System.Collections.Generic;

namespace Nordigen.Net.Responses
{
    using Newtonsoft.Json;

    public class Account
    {
        [JsonConstructor]
        internal Account(string id, DateTime created, DateTime last_accessed, string iban, string institution_id, IReadOnlyDictionary<string, string> status)
        {
            Id = id;
            Created = created;
            LastAccessed = last_accessed;
            IBAN = iban;
            InstitutionId = institution_id;
            Status = status;
        }

        public string Id { get; }

        public DateTime Created { get; }

        public DateTime LastAccessed { get; }

        public string IBAN { get; }

        public string InstitutionId { get; }

        public IReadOnlyDictionary<string, string> Status { get; }
    }
}
