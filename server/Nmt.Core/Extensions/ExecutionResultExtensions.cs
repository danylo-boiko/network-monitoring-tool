using Grpc.Core;
using HotChocolate;
using LS.Helpers.Hosting.API;

namespace Nmt.Core.Extensions;

public static class ExecutionResultExtensions
{
    public static RpcException ToGrpcException(this ExecutionResult executionResult, StatusCode statusCode)
    {
        return new RpcException(new Status(statusCode, executionResult.ErrorMessage()));
    }

    public static GraphQLException ToGraphQLException(this ExecutionResult executionResult)
    {
        return new GraphQLException(executionResult.ErrorMessage());
    }

    private static string ErrorMessage(this ExecutionResult executionResult)
    {
        return string.Join("; ", executionResult.Errors.Select(e => e.ErrorMessage));
    }
}