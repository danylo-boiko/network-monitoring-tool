using System.Security.Claims;
using MediatR;
using Nmt.Core.CQRS.Commands.IpFilters.CreateIpFilter;
using Nmt.Core.CQRS.Commands.IpFilters.DeleteIpFilter;
using Nmt.Core.CQRS.Commands.IpFilters.UpdateIpFilter;
using Nmt.Core.Extensions;
using Nmt.Domain.Consts;
using Nmt.Domain.Enums;
using Nmt.GraphQL.Attributes;
using Nmt.GraphQL.Consts;

namespace Nmt.GraphQL.Mutations;

[ExtendObjectType(ObjectTypes.Mutation)]
public class IpFilters
{
    [PermissionsAuthorize(Permission.IpFiltersCreate)]
    public async Task<Guid> CreateIpFilter([Service] IMediator mediator, CreateIpFilterCommand input, ClaimsPrincipal claims)
    {
        input.UserId = claims.FindFirstGuid(AuthClaims.UserId);

        return await mediator.Send(input);
    }

    [PermissionsAuthorize(Permission.IpFiltersUpdate)]
    public async Task<bool> UpdateIpFilter([Service] IMediator mediator, UpdateIpFilterCommand input)
    {
        return await mediator.Send(input);
    }

    [PermissionsAuthorize(Permission.IpFiltersDelete)]
    public async Task<bool> DeleteIpFilter([Service] IMediator mediator, DeleteIpFilterCommand input)
    {
        return await mediator.Send(input);
    }
}