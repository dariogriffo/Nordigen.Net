[![build and test](https://github.com/dariogriffo/Nordigen.Net/actions/workflows/build.yml/badge.svg)](https://github.com/dariogriffo/Nordigen.Net/actions/workflows/build.yml)
[![NuGet](https://img.shields.io/nuget/v/Nordigen.Net.svg?style=flat)](https://www.nuget.org/packages/Nordigen.Net/) 
[![GitHub license](https://img.shields.io/github/license/dariogriffo/Nordigen.Net.svg)](https://raw.githubusercontent.com/dariogriffo/Nordigen.Net/master/LICENSE)

[![N|Solid](https://avatars2.githubusercontent.com/u/39886363?s=200&v=4)](https://github.com/dariogriffo/Nordigen.Net)

# Nordigen.Net
Unofficial .NET SDK for Nordigen.
Work still in progress

# How to use it

1- Install the nuget package

`Install-Package Nordigen.Net`

2- Add a configuration section into your app, and configure it (the values shown below are the default ones except the keys)
```json
"NordigenApi": {
    "Url" : "https://ob.nordigen.com/",
    "SecretId" : "SOME_SECRET_ID_HERE",
    "SecretKey" : "SOME_SECRET_KEY_HERE",
    "AccessTokenValidBeforeSeconds" : 5,
    "RefreshTokenValidBeforeSeconds" : 5
}
```

Although this is not required is recommended if you want to use an internal api to test.

3- Add the SDK to your services collection

```csharp
services.AddNordigenApi();
```

4- Now you can access a set of interfaces to reach the Nordigen platform
```csharp
INordigenApi => Just inject this and you can have access to all the endpoints with the members
IAccountsEndpoint 2=> Access to Accounts
IInstitutionsEndpoint => Access to Institutions
IRequisitionsEndpoint => Access to Requisitions
```

# Retries

You can easily plug retries with [Polly](https://github.com/App-vNext/Polly) when registering the api.
Install the nuget package

`Install-Package Microsoft.Extensions.Http.Polly`

Configure your retries:
```csharp
services
  .AddNordigenApi()
  .AddPolicyHandler(
      HttpPolicyExtensions
        .HandleTransientHttpError()
        .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.ServiceUnavailable)
        .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2,retryAttempt))));
```



# Result handling
The library uses discriminated unions to return values, when the second option will allways be an Error if present. 

For example when querying accounts:
```csharp
var result = await _accountsEndpoint.Get(id, cancellationToken);

_ = result.Match(
	   account => async _ => { await _accountsDatabase.SaveAsync(account, cancellationToken); },
	   error => { _logger.Error(error.Detail); }
	);
```

# Token management

The SDK handles the lifetime/renewal of tokens automatically for you, so only requirement is to configure your SecretId and SecretKey

# Credits

OneOf simplified credit goes to [Harry McIntyre](https://github.com/mcintyre321/OneOf) 

Logo Provided by [Vecteezy](https://vecteezy.com)
