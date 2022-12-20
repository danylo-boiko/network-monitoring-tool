using Grpc.Core;
using MediatR;
using Nmt.Core.Commands.Auth;
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
        var jwtToken = await _mediator.Send(new LoginCommand
        {
            Username = request.Username,
            Password = request.Password,
            Hostname = request.Hostname,
            MachineSpecificStamp = request.MachineSpecificStamp
        }, context.CancellationToken);

        return new AuthResponse
        {
            Token = jwtToken
        };
    }
}