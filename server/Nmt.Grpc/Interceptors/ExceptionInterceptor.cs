using FluentValidation;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Nmt.Domain.Consts;
using Nmt.Domain.Exceptions;

namespace Nmt.Grpc.Interceptors;

public class ExceptionInterceptor : Interceptor
{
    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request, 
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            return await continuation(request, context);
        }
        catch (ValidationException e)
        {
            var message = string.Join("; ", e.Errors.Select(err => err.ErrorMessage));
            throw new RpcException(new Status(StatusCode.InvalidArgument, message));
        }
        catch (DomainException e)
        {
            throw new RpcException(new Status(DomainExceptionCodeToRpcStatusCode(e.Code), e.Message));
        }
        catch (Exception e)
        {
            throw new RpcException(new Status(StatusCode.Internal, e.Message));
        }
    }

    private StatusCode DomainExceptionCodeToRpcStatusCode(string code)
    {
        return code switch
        {
            ExceptionCodes.NotFound => StatusCode.NotFound,
            ExceptionCodes.EmailConfirmation => StatusCode.Unauthenticated,
            ExceptionCodes.WrongData => StatusCode.InvalidArgument,
            ExceptionCodes.AlreadyExists => StatusCode.AlreadyExists,
            _ => StatusCode.Internal
        };
    }
}