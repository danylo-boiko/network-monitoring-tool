namespace Nmt.Domain.Configs;

public class JwtConfig
{
    public string ValidIssuer { get; set; }
    public string Secret { get; set; }
    public uint AccessTokenExpiresInHours { get; set; }
    public uint RefreshTokenExpiresInDays { get; set; }
}