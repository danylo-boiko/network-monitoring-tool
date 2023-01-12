using LS.Helpers.Hosting.API;
using MediatR;
using Nmt.Domain.Common;

namespace Nmt.GraphQL.Services.Interfaces;

public interface IExecutionResultService : IService
{
    public Task<T> HandleExecutionResultRequestAsync<T>(IRequest<ExecutionResult<T>> request);
}