using MediatR;
using Nmt.Domain.Models;

namespace Nmt.Core.CQRS.Commands.Auth.CreateToken;

public record CreateTokenCommand : IRequest<string>
{
    public User User { get; set; }
    public Guid? DeviceId { get; set; }
}
