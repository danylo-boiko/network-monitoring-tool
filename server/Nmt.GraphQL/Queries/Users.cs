using System.Security.Claims;
using MediatR;
using Nmt.Core.CQRS.Queries.Users.GetUserWithDevicesAndIpFiltersById;
using Nmt.Core.Extensions;
using Nmt.Domain.Consts;
using Nmt.Domain.Enums;
using Nmt.GraphQL.Attributes;
using Nmt.GraphQL.Consts;

namespace Nmt.GraphQL.Queries;

[ExtendObjectType(ObjectTypes.Query)]
public class Users
{
    [PermissionsAuthorize(Permission.UsersRead)]
    public async Task<UserDto> GetUserInfo([Service] IMediator mediator, ClaimsPrincipal claims)
    {
        return await mediator.Send(new GetUserWithDevicesAndIpFiltersByIdQuery
        {
            UserId = claims.FindFirstGuid(AuthClaims.UserId)
        });
    }
}