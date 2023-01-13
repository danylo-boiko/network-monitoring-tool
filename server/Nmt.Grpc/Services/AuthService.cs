using Grpc.Core;
using MediatR;
using Nmt.Core.CQRS.Commands.Auth.Login;
using Nmt.Core.CQRS.Commands.Auth.RefreshToken;
using Nmt.Core.Extensions;
using Nmt.Grpc.Protos;

namespace Nmt.Grpc.Services;

public class AuthService : Auth.AuthBase
{
    private readonly IMediator _mediator;

    public AuthService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override async Task<AuthResponse> Login(LoginRequest request, ServerCallContext context)
    {
        var tokenResult = await _mediator.Send(new LoginCommand
        {
            Username = request.Username,
            Password = request.Password,
            Hostname = request.Hostname,
            MachineSpecificStamp = request.MachineSpecificStamp
        }, context.CancellationToken);

        if (!tokenResult.Success)
        {
            throw tokenResult.ToGrpcException();
        }

        return new AuthResponse
        {
            AccessToken = tokenResult.Value.AccessToken,
            RefreshToken = tokenResult.Value.RefreshToken
        };
    }

    public override async Task<AuthResponse> RefreshToken(RefreshTokenRequest request, ServerCallContext context)
    {
        var tokenResult = await _mediator.Send(new RefreshTokenCommand
        {
            AccessToken = request.AccessToken,
            RefreshToken = request.RefreshToken
        }, context.CancellationToken);

        if (!tokenResult.Success)
        {
            throw tokenResult.ToGrpcException();
        }

        return new AuthResponse
        {
            AccessToken = tokenResult.Value.AccessToken,
            RefreshToken = tokenResult.Value.RefreshToken
        };
    }
}