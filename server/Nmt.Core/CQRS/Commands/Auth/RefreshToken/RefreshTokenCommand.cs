using LS.Helpers.Hosting.API;
using MediatR;
using Nmt.Core.CQRS.Commands.Auth.Login;

namespace Nmt.Core.CQRS.Commands.Auth.RefreshToken;

public record RefreshTokenCommand : IRequest<ExecutionResult<TokenDto>>
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}