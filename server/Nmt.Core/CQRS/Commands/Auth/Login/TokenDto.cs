namespace Nmt.Core.CQRS.Commands.Auth.Login;

public class TokenDto
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}