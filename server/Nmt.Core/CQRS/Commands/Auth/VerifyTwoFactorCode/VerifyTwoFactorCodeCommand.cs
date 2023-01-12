using LS.Helpers.Hosting.API;
using MediatR;

namespace Nmt.Core.CQRS.Commands.Auth.VerifyTwoFactorCode;

public record VerifyTwoFactorCodeCommand : IRequest<ExecutionResult<bool>>
{
    public string Email { get; set; }
    public string TwoFactorCode { get; set; }
}