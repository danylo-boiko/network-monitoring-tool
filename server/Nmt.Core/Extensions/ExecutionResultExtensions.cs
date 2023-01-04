using Grpc.Core;
using HotChocolate;
using LS.Helpers.Hosting.API;

namespace Nmt.Core.Extensions;

public static class ExecutionResultExtensions
{
    public static RpcException ToGrpcException(this ExecutionResult executionResult)
    {
        var statusCode = executionResult.Errors.First().Key;
        var grpcStatusCode = ToGrpcStatusCode(statusCode);

        return new RpcException(new Status(grpcStatusCode, executionResult.ErrorMessage()));
    }

    public static GraphQLException ToGraphQLException(this ExecutionResult executionResult)
    {
        return new GraphQLException(executionResult.ErrorMessage());
    }

    private static StatusCode ToGrpcStatusCode(string statusCode)
    {
        if (Enum.TryParse(statusCode, true, out StatusCode grpcStatusCode))
        {
            return grpcStatusCode;
        }

        return StatusCode.Internal;
    }

    private static string ErrorMessage(this ExecutionResult executionResult)
    {
        return string.Join("; ", executionResult.Errors.Select(e => e.ErrorMessage));
    }
}