namespace Nordigen.Net.Internal.Model;

using Newtonsoft.Json;
using Responses;

internal class AccountDetailsHolder
{
    [JsonConstructor]
    public AccountDetailsHolder(AccountDetails account)
    {
        Account = account;
    }

    public AccountDetails Account { get; }
}