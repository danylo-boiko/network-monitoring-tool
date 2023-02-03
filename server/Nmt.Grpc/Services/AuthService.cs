using Grpc.Core;
using MediatR;
using Nmt.Core.CQRS.Commands.Auth.Login;
using Nmt.Core.CQRS.Commands.Auth.RefreshToken;
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
        var tokens= await _mediator.Send(new LoginCommand
        {
            Username = request.Username,
            Password = request.Password,
            Hostname = request.Hostname,
            MachineSpecificStamp = request.MachineSpecificStamp
        }, context.CancellationToken);

        return new AuthResponse
        {
            AccessToken = tokens.AccessToken,
            RefreshToken = tokens.RefreshToken
        };
    }

    public override async Task<AuthResponse> RefreshToken(RefreshTokenRequest request, ServerCallContext context)
    {
        var tokens = await _mediator.Send(new RefreshTokenCommand
        {
            AccessToken = request.AccessToken,
            RefreshToken = request.RefreshToken
        }, context.CancellationToken);

        return new AuthResponse
        {
            AccessToken = tokens.AccessToken,
            RefreshToken = tokens.RefreshToken
        };    
    }
}