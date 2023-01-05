using Nmt.Core.CQRS.Queries.Users.GetUserWithDevicesAndIpFiltersById;
using Nmt.Domain.Enums;
using Nmt.GraphQL.Attributes;
using Nmt.GraphQL.Consts;
using Nmt.GraphQL.Services.Interfaces;

namespace Nmt.GraphQL.Queries;

[ExtendObjectType(ObjectTypes.Query)]
public class Users
{
    [PermissionsAuthorize(Permission.UsersRead)]
    public async Task<UserDto> GetUserById([Service] IExecutionResultService executionResultService, GetUserWithDevicesAndIpFiltersByIdQuery input)
    {
        return await executionResultService.HandleExecutionResultRequest(input);
    }
}