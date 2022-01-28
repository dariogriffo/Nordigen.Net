[![build and test](https://github.com/dariogriffo/Nordigen.Net/actions/workflows/build.yml/badge.svg)](https://github.com/dariogriffo/Nordigen.Net/actions/workflows/build.yml)
[![NuGet](https://img.shields.io/nuget/v/Nordigen.Net.svg?style=flat)](https://www.nuget.org/packages/Nordigen.Net/) 
[![GitHub license](https://img.shields.io/github/license/dariogriffo/Nordigen.Net.svg)](https://raw.githubusercontent.com/dariogriffo/Nordigen.Net/master/LICENSE)

[![N|Solid](https://avatars2.githubusercontent.com/u/39886363?s=200&v=4)](https://github.com/dariogriffo/Nordigen.Net)

# Nordigen.Net
Unofficial .NET SDK for Nordigen

# How to use it

1- Install the nuget package

`Install-Package Nordigen.Net`

2- Add a configuration section into your app, and configure it (the values shown below are the default ones except the keys)
```
"NordigenApi": {
	"Url" : "https://ob.nordigen.com/",
	"SecretId" : "SOME_SECRET_ID_HERE",
	"SecretKey" : "SOME_SECRET_KEY_HERE",
    "AccessTokenValidBeforeSeconds" : 5,
    "RefreshTokenValidBeforeSeconds" : 5;
}
```

Although this is not required is recommended if you want to use an internal api to test.

3- Add the SDK to your services collection

```
    services.AddNordigenApi();
```

4- Now you can access a set of interfaces to reach the Nordigen platform
```

INordigenApi => Just inject this and you can have access to all the endpoints with the members
IAccountsEndpoint => Access to Accounts
IInstitutionsEndpoint => Access to Institutions
```

# 
Logo Provided by [Vecteezy](https://vecteezy.com)
