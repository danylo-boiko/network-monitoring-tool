using HotChocolate.AspNetCore.Authorization;
using MediatR;
using Nmt.Core.CQRS.Commands.IpFilters.CreateIpFilter;
using Nmt.Core.CQRS.Commands.IpFilters.DeleteIpFilter;
using Nmt.Core.Extensions;

namespace Nmt.GraphQL.Mutations;

[Authorize]
[ExtendObjectType(ObjectTypes.Mutation)]
public class IpFilters
{
    public async Task CreateIpFilter([Service] IMediator mediator, CreateIpFilterCommand input)
    {
        await mediator.Send(input);
    }

    public async Task DeleteIpFilter([Service] IMediator mediator, DeleteIpFilterCommand input)
    {
        var executionResult = await mediator.Send(input);

        if (!executionResult.Success)
        {
            throw executionResult.ToGraphQLException();
        }
    }
}