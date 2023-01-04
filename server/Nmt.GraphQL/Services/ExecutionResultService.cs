using LS.Helpers.Hosting.API;
using MediatR;
using Nmt.Core.Extensions;
using Nmt.GraphQL.Services.Interfaces;

namespace Nmt.GraphQL.Services;

public class ExecutionResultService : IExecutionResultService
{
    private readonly IMediator _mediator;

    public ExecutionResultService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<T> HandleExecutionResultRequest<T>(IRequest<ExecutionResult<T>> request)
    {
        var requestResult = await _mediator.Send(request);

        if (!requestResult.Success)
        {
            throw requestResult.ToGraphQLException();
        }

        return requestResult.Value;
    }
}