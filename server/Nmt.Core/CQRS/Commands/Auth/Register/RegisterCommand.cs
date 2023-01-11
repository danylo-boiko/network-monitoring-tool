using LS.Helpers.Hosting.API;
using MediatR;

namespace Nmt.Core.CQRS.Commands.Auth.Register;

public record RegisterCommand : IRequest<ExecutionResult<bool>>
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}
