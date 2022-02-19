namespace Nordigen.Net.Internal;

using Responses;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

internal class InstitutionsEndpoint : IInstitutionsEndpoint
{
    private static readonly HashSet<string> CountryList = new HashSet<string>()
    {
        "AF","AL","DZ","AS","AD","AO","AI","AQ","AG","AR","AM","AW","AU","AT","AZ","BS","BH","BD","BB","BY",
        "BE","BZ","BJ","BM","BT","BO","BQ","BA","BW","BV","BR","IO","BN","BG","BF","BI","CV","KH","CM","CA",
        "KY","CF","TD","CL","CN","CX","CC","CO","KM","CG","CD","CK","CR","CI","HR","CU","CW","CY","CZ","DK",
        "DJ","DM","DO","EC","EG","SV","GQ","ER","EE","SZ","ET","FK","FO","FJ","FI","FR","GF","PF","TF","GA",
        "GM","GE","DE","GH","GI","GR","GL","GD","GP","GU","GT","GG","GN","GW","GY","HT","HM","VA","HN","HK",
        "HU","IS","IN","ID","IR","IQ","IE","IM","IL","IT","JM","JP","JE","JO","KZ","KE","KI","KP","KR","KW",
        "KG","LA","LV","LB","LS","LR","LY","LI","LT","LU","MO","MG","MW","MY","MV","ML","MT","MH","MQ","MR",
        "MU","YT","MX","FM","MD","MC","MN","ME","MS","MA","MZ","MM","NA","NR","NP","NL","NC","NZ","NI","NE",
        "NG","NU","NF","MP","MK","NO","OM","PK","PW","PS","PA","PG","PY","PE","PH","PN","PL","PT","PR","QA",
        "RE","RO","RU","RW","BL","SH","KN","LC","MF","PM","VC","WS","SM","ST","SA","SN","RS","SC","SL","SG",
        "SX","SK","SI","SB","SO","ZA","GS","SS","ES","LK","SD","SR","SJ","SE","CH","SY","TW","TJ","TZ","TH",
        "TL","TG","TK","TO","TT","TN","TR","TM","TC","TV","UG","UA","AE","GB","US","UM","UY","UZ","VU","VE",
        "VN","VG","VI","WF","EH","YE","ZM","ZW","AX"
    };

    private readonly INordigenHttpClient _client;

    public InstitutionsEndpoint(INordigenHttpClient client)
    {
        _client = client;
    }

    public Task<NOneOf<Institution[], Error>> GetByCountryIso3166Code(string country, CancellationToken cancellationToken = default)
    {
        if (!CountryList.Contains(country))
        {
            throw new ArgumentOutOfRangeException(nameof(country), "Unknown country. Please check the official ISO country codes");
        }

        return _client.Get<Institution[]>($"api/v2/institutions/?country={country}", cancellationToken);
    }

    public Task<NOneOf<Institution, Error>> Get(Guid id, CancellationToken cancellationToken = default)
    {
        return _client.Get<Institution>($"api/v2/institutions/{id}/", cancellationToken);
    }
}