using HotChocolate.AspNetCore.Authorization;
using Nmt.Core.CQRS.Queries.Users.GetUserById;
using Nmt.GraphQL.Consts;
using Nmt.GraphQL.Services.Interfaces;

namespace Nmt.GraphQL.Queries;

[Authorize]
[ExtendObjectType(ObjectTypes.Query)]
public class Users
{
    public async Task<UserDto> GetUserById([Service] IExecutionResultService executionResultService, GetUserByIdQuery input)
    {
        return await executionResultService.HandleExecutionResultRequest(input);
    }
}