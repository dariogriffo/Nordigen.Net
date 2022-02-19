namespace Nordigen.Net;

public class NordigenApiOptions
{
    public string Url { get; set; } = "https://ob.nordigen.com/";

    public string SecretId { get; set; } = string.Empty;

    public string SecretKey { get; set; } = string.Empty;

    public int AccessTokenValidBeforeSeconds { get; set; } = 5;

    public int RefreshTokenValidBeforeSeconds { get; set; } = 5;
}