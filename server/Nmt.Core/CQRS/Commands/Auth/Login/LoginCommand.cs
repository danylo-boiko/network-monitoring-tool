using LS.Helpers.Hosting.API;
using MediatR;
using Nmt.Core.CQRS.Commands.Auth.RefreshToken;

namespace Nmt.Core.CQRS.Commands.Auth.Login;

public record LoginCommand : IRequest<ExecutionResult<TokenDto>>
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string? Hostname { get; set; }
    public string? MachineSpecificStamp { get; set; }
}
