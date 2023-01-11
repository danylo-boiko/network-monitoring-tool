using LS.Helpers.Hosting.API;
using MediatR;

namespace Nmt.Core.CQRS.Commands.Auth.SendTwoFactorCode;

public record SendTwoFactorCodeCommand : IRequest<ExecutionResult<bool>>
{
    public string Email { get; set; }
}