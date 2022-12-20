namespace Nmt.Domain.Configs;

public class JwtConfig
{
    public string ValidIssuer { get; set; }
    public string Secret { get; set; }
    public uint ExpiresIn { get; set; }
}