using MediatR;
using Nmt.Domain.Models;

namespace Nmt.Core.CQRS.Commands.Auth.CreateToken;

public record CreateTokenCommand : IRequest<string>
{
    public Guid UserId { get; set; }
    public Guid? DeviceId { get; set; }
}
