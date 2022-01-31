# Nordigen.Net
Unofficial .NET SDK for Nordigen

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
    "RefreshTokenValidBeforeSeconds" : 5;
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
IAccountsEndpoint => Access to Accounts
IInstitutionsEndpoint => Access to Institutions
```

# Token management

The SDK handles the lifetime/renewal of tokens automatically for you, so only requirement is to configure your SecretId and SecretKey

# Credits

OneOf simplified credit goes to [Harry McIntyre](https://github.com/mcintyre321/OneOf) 

Logo Provided by [Vecteezy](https://vecteezy.com)
