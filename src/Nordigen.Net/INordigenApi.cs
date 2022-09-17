namespace Nordigen.Net;

public interface INordigenApi
{
    IAccountsEndpoint Accounts { get; }

    IInstitutionsEndpoint Institutions { get; }

    IAgreementsEndpoint Agreements { get; }

    IRequisitionsEndpoint Requisitions { get; }

}
