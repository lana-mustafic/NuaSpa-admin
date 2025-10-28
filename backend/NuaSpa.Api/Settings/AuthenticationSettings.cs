namespace NuaSpa.Api.Settings;

public class AuthenticationSettings
{
    public string JwtKey { get; set; } = null!;
    public string JwtIssuer { get; set; } = null!;
    public int JwtExpireMinutes { get; set; }
}
