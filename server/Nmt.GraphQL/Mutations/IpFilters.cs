using System.Security.Claims;
using Nmt.Core.CQRS.Commands.IpFilters.CreateIpFilter;
using Nmt.Core.CQRS.Commands.IpFilters.DeleteIpFilter;
using Nmt.Core.CQRS.Commands.IpFilters.UpdateIpFilter;
using Nmt.Core.Extensions;
using Nmt.Domain.Consts;
using Nmt.Domain.Enums;
using Nmt.GraphQL.Attributes;
using Nmt.GraphQL.Consts;
using Nmt.GraphQL.Services.Interfaces;

namespace Nmt.GraphQL.Mutations;

[ExtendObjectType(ObjectTypes.Mutation)]
public class IpFilters
{
    [PermissionsAuthorize(Permission.IpFiltersCreate)]
    public async Task<Guid> CreateIpFilter([Service] IExecutionResultService service, CreateIpFilterCommand input, ClaimsPrincipal claims)
    {
        input.UserId = claims.FindFirstGuid(AuthClaims.UserId);
        return await service.HandleExecutionResultRequestAsync(input);
    }

    [PermissionsAuthorize(Permission.IpFiltersUpdate)]
    public async Task<bool> UpdateIpFilter([Service] IExecutionResultService service, UpdateIpFilterCommand input)
    {
        return await service.HandleExecutionResultRequestAsync(input);
    }

    [PermissionsAuthorize(Permission.IpFiltersDelete)]
    public async Task<bool> DeleteIpFilter([Service] IExecutionResultService service, DeleteIpFilterCommand input)
    {
        return await service.HandleExecutionResultRequestAsync(input);
    }
}