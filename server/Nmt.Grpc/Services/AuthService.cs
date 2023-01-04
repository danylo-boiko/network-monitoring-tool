using Grpc.Core;
using MediatR;
using Nmt.Core.CQRS.Commands.Auth.Login;
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
        var jwtTokenResult = await _mediator.Send(new LoginCommand
        {
            Username = request.Username,
            Password = request.Password,
            Hostname = request.Hostname,
            MachineSpecificStamp = request.MachineSpecificStamp
        }, context.CancellationToken);

        if (!jwtTokenResult.Success)
        {
            throw jwtTokenResult.ToGrpcException();
        }

        return new AuthResponse
        {
            Token = jwtTokenResult.Value
        };
    }
}