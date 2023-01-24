using AppAny.HotChocolate.FluentValidation;
using Nmt.Core.CQRS.Commands.IpFilters.CreateIpFilter;
using Nmt.Core.CQRS.Commands.IpFilters.DeleteIpFilter;
using Nmt.Core.CQRS.Commands.IpFilters.UpdateIpFilter;
using Nmt.Domain.Enums;
using Nmt.GraphQL.Attributes;
using Nmt.GraphQL.Consts;
using Nmt.GraphQL.Services.Interfaces;

namespace Nmt.GraphQL.Mutations;

[ExtendObjectType(ObjectTypes.Mutation)]
public class IpFilters
{
    [PermissionsAuthorize(Permission.IpFiltersCreate)]
    public async Task<Guid> CreateIpFilter(
        [Service] IExecutionResultService executionResultService, 
        [UseFluentValidation] CreateIpFilterCommand input)
    {
        return await executionResultService.HandleExecutionResultRequestAsync(input);
    }

    [PermissionsAuthorize(Permission.IpFiltersUpdate)]
    public async Task<bool> UpdateIpFilter(
        [Service] IExecutionResultService executionResultService, 
        [UseFluentValidation] UpdateIpFilterCommand input)
    {
        return await executionResultService.HandleExecutionResultRequestAsync(input);
    }

    [PermissionsAuthorize(Permission.IpFiltersDelete)]
    public async Task<bool> DeleteIpFilter(
        [Service] IExecutionResultService executionResultService, 
        [UseFluentValidation] DeleteIpFilterCommand input)
    {
        return await executionResultService.HandleExecutionResultRequestAsync(input);
    }
}