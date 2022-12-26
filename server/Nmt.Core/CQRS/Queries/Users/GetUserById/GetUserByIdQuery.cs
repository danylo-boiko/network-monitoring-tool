using MediatR;

namespace Nmt.Core.CQRS.Queries.Users.GetUserById;

public record GetUserByIdQuery : IRequest<UserDto>
{
    public Guid UserId { get; set; }
}
