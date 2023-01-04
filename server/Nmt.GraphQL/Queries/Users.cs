using Nmt.Core.CQRS.Queries.Users.GetUserById;
using Nmt.Domain.Enums;
using Nmt.GraphQL.Attributes;
using Nmt.GraphQL.Consts;
using Nmt.GraphQL.Services.Interfaces;

namespace Nmt.GraphQL.Queries;

[ExtendObjectType(ObjectTypes.Query)]
public class Users
{
    [PermissionsAuthorize(Permission.UsersRead)]
    public async Task<UserDto> GetUserById([Service] IExecutionResultService executionResultService, GetUserByIdQuery input)
    {
        return await executionResultService.HandleExecutionResultRequest(input);
    }
}