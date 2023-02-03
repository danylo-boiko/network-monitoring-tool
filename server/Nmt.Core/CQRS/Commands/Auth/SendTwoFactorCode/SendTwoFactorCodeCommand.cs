using MediatR;

namespace Nmt.Core.CQRS.Commands.Auth.SendTwoFactorCode;

public record SendTwoFactorCodeCommand : IRequest<bool>
{
    public string Username { get; set; }
}