using HotChocolate.AspNetCore.Authorization;
using MediatR;
using Nmt.Core.CQRS.Queries.Users.GetUserById;
using Nmt.Core.Extensions;

namespace Nmt.GraphQL.Queries;

[Authorize]
[ExtendObjectType("Query")]
public class Users
{
    public async Task<UserDto> GetUserById([Service] IMediator mediator, GetUserByIdQuery input)
    {
        var userResult = await mediator.Send(input);

        if (!userResult.Success)
        {
            throw userResult.ToGraphQLException();
        }

        return userResult.Value;
    }
}