using Nmt.Core.CQRS.Commands.IpFilters.CreateIpFilter;
using Nmt.Core.CQRS.Commands.IpFilters.DeleteIpFilter;
using Nmt.Domain.Enums;
using Nmt.GraphQL.Attributes;
using Nmt.GraphQL.Consts;
using Nmt.GraphQL.Services.Interfaces;

namespace Nmt.GraphQL.Mutations;

[ExtendObjectType(ObjectTypes.Mutation)]
public class IpFilters
{
    [PermissionsAuthorize(Permission.IpFiltersCreate)]
    public async Task<Guid> CreateIpFilter([Service] IExecutionResultService executionResultService, CreateIpFilterCommand input)
    {
        return await executionResultService.HandleExecutionResultRequestAsync(input);
    }

    [PermissionsAuthorize(Permission.IpFiltersDelete)]
    public async Task<bool> DeleteIpFilter([Service] IExecutionResultService executionResultService, DeleteIpFilterCommand input)
    {
        return await executionResultService.HandleExecutionResultRequestAsync(input);
    }
}