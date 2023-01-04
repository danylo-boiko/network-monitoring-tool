using HotChocolate.AspNetCore.Authorization;
using Nmt.Core.CQRS.Commands.IpFilters.CreateIpFilter;
using Nmt.Core.CQRS.Commands.IpFilters.DeleteIpFilter;
using Nmt.GraphQL.Consts;
using Nmt.GraphQL.Services.Interfaces;

namespace Nmt.GraphQL.Mutations;

[Authorize]
[ExtendObjectType(ObjectTypes.Mutation)]
public class IpFilters
{
    public async Task<Guid> CreateIpFilter([Service] IExecutionResultService executionResultService, CreateIpFilterCommand input)
    {
        return await executionResultService.HandleExecutionResultRequest(input);
    }

    public async Task<bool> DeleteIpFilter([Service] IExecutionResultService executionResultService, DeleteIpFilterCommand input)
    {
        return await executionResultService.HandleExecutionResultRequest(input);
    }
}