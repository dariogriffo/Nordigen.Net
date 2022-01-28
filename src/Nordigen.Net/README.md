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
