using MediatR;
using Nmt.Core.CQRS.Queries.Users.GetUserById;

namespace Nmt.GraphQL.Queries;

public class Users
{
    private readonly IMediator _mediator;

    public Users(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<UserDto> GetUserById(GetUserByIdQuery input) => await _mediator.Send(input);
}