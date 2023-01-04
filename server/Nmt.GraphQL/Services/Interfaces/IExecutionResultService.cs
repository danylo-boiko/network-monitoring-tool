using LS.Helpers.Hosting.API;
using MediatR;

namespace Nmt.GraphQL.Services.Interfaces;

public interface IExecutionResultService
{
    public Task<T> HandleExecutionResultRequest<T>(IRequest<ExecutionResult<T>> request);
}