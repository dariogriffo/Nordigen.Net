namespace Nordigen.Net.Internal
{
    public class NordigenApi : INordigenApi
    {
        public NordigenApi(
            IAccountsEndpoint accounts,
            IInstitutionsEndpoint institutions,
            IRequisitionsEndpoint requisitions)
        {
            Accounts = accounts;
            Institutions = institutions;
            Requisitions = requisitions;
        }

        public IAccountsEndpoint Accounts { get; }

        public IInstitutionsEndpoint Institutions { get; }

        public IRequisitionsEndpoint Requisitions { get; }
    }
}
