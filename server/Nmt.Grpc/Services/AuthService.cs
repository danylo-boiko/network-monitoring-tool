using Grpc.Core;
using MediatR;
using Nmt.Core.Commands.Auth;
using Nmt.Grpc.Protos;

namespace Nmt.Grpc.Services;

public class AuthService : Auth.AuthBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<AuthService> _logger;

    public AuthService(IMediator mediator, ILogger<AuthService> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public override async Task<AuthResponse> Login(LoginRequest request, ServerCallContext context)
    {
        var authResponse = await _mediator.Send(new LoginCommand
        {
            Username = request.Username,
            Password = request.Password,
            Hostname = request.Hostname,
            MachineSpecificStamp = request.MachineSpecificStamp
        }, context.CancellationToken);

        return new AuthResponse
        {
            Token = authResponse
        };
    }
}