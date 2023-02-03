using MediatR;
using Nmt.Core.CQRS.Commands.Auth.Login;

namespace Nmt.Core.CQRS.Commands.Auth.RefreshToken;

public record RefreshTokenCommand : IRequest<TokenDto>
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}