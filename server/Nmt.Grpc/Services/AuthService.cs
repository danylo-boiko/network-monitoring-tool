using Grpc.Core;
using Nmt.Grpc.Protos;

namespace Nmt.Grpc.Services;

public class AuthService : Auth.AuthBase
{
    private readonly ILogger<AuthService> _logger;

    public AuthService(ILogger<AuthService> logger)
    {
        _logger = logger;
    }

    public override async Task<AuthResponse> Login(LoginRequest request, ServerCallContext context)
    {
        return new AuthResponse
        {
            Token = String.Empty
        };
    }
}