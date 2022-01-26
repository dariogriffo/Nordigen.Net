namespace Nordigen.Net.Internal
{
    public class NordigenApi : INordigenApi
    {
        public NordigenApi(
            IAccountsEndpoint accounts, 
            IInstitutionsEndpoint institutions)
        {
            Accounts = accounts;
            Institutions = institutions;
        }

        public IAccountsEndpoint Accounts { get; }
        
        public IInstitutionsEndpoint Institutions { get; }
    }
}
