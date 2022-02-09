namespace Nordigen.Net
{
    public interface INordigenApi
    {
        IAccountsEndpoint Accounts { get; }

        IInstitutionsEndpoint Institutions { get; }

        IRequisitionsEndpoint Requisitions { get; }

    }
}
