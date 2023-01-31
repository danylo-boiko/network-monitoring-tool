using System.Security.Claims;
using Nmt.Core.CQRS.Queries.Users.GetUserWithDevicesAndIpFiltersById;
using Nmt.Core.Extensions;
using Nmt.Domain.Consts;
using Nmt.Domain.Enums;
using Nmt.GraphQL.Attributes;
using Nmt.GraphQL.Consts;
using Nmt.GraphQL.Services.Interfaces;

namespace Nmt.GraphQL.Queries;

[ExtendObjectType(ObjectTypes.Query)]
public class Users
{
    [PermissionsAuthorize(Permission.UsersRead)]
    public async Task<UserDto> GetUserInfo(
        [Service] IExecutionResultService executionResultService, 
        ClaimsPrincipal claims)
    {
        var query = new GetUserWithDevicesAndIpFiltersByIdQuery
        {
            UserId = claims.FindFirstGuid(AuthClaims.UserId)
        };

        return await executionResultService.HandleExecutionResultRequestAsync(query);
    }
}