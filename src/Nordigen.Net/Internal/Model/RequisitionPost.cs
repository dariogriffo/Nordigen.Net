namespace Nordigen.Net.Internal.Model
{
    using Newtonsoft.Json;
    using System;

    internal class RequisitionPost
    {
        public RequisitionPost(
            string redirect,
            Guid institutionId,
            Guid agreement,
            string reference,
            string userLanguage,
            string ssn,
            bool accountSelection
        )
        {
            this.Redirect = redirect;
            this.InstitutionId = institutionId;
            this.Agreement = agreement;
            this.Reference = reference;
            this.UserLanguage = userLanguage;
            this.Ssn = ssn;
            this.AccountSelection = accountSelection;
        }

        [JsonProperty("redirect")]
        public string Redirect { get; }

        [JsonProperty("institution_id")]
        public Guid InstitutionId { get; }

        [JsonProperty("agreement")]
        public Guid Agreement { get; }

        [JsonProperty("reference")]
        public string Reference { get; }

        [JsonProperty("user_language")]
        public string UserLanguage { get; }

        [JsonProperty("ssn")]
        public string Ssn { get; }

        [JsonProperty("account_selection")]
        public bool AccountSelection { get; }
    }
}
