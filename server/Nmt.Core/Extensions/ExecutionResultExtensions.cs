using Grpc.Core;
using HotChocolate;
using LS.Helpers.Hosting.API;

namespace Nmt.Core.Extensions;

public static class ExecutionResultExtensions
{
    public static readonly string GraphQLPropertyName = "property";

    public static RpcException ToGrpcException(this ExecutionResult executionResult)
    {
        return new RpcException(new Status(StatusCode.Internal, executionResult.ErrorMessage()));
    }

    public static GraphQLException ToGraphQLException(this ExecutionResult executionResult)
    {
        var errors = executionResult.Errors
            .Select(e => new Error(message: e.ErrorMessage, extensions: new Dictionary<string, object?>
            {
                { GraphQLPropertyName, e.Key }
            }))
            .ToList();

        return new GraphQLException(errors);
    }

    private static string ErrorMessage(this ExecutionResult executionResult)
    {
        return string.Join("; ", executionResult.Errors.Select(e => e.ErrorMessage));
    }
}