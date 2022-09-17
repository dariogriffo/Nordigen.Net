using System;

namespace Nordigen.Net.Internal;

internal class NordigenApi : INordigenApi
{
    public NordigenApi(
        IAccountsEndpoint accounts,
        IInstitutionsEndpoint institutions,
        IRequisitionsEndpoint requisitions,
        IAgreementsEndpoint agreements)
    {
        Accounts = accounts ?? throw new ArgumentNullException(nameof(accounts));
        Institutions = institutions ?? throw new ArgumentNullException(nameof(institutions));
        Requisitions = requisitions ?? throw new ArgumentNullException(nameof(requisitions));
        Agreements = agreements ?? throw new ArgumentNullException(nameof(agreements));
    }

    public IAccountsEndpoint Accounts { get; }

    public IInstitutionsEndpoint Institutions { get; }
    
    public IAgreementsEndpoint Agreements { get; }

    public IRequisitionsEndpoint Requisitions { get; }
}