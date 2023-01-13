using LS.Helpers.Hosting.API;
using MediatR;
using Nmt.Core.CQRS.Commands.Auth.Login;
using Nmt.Core.Services.Interfaces;

namespace Nmt.Core.CQRS.Commands.Auth.RefreshToken;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, ExecutionResult<TokenDto>>
{
    private readonly ITokenService _tokenService;

    public RefreshTokenCommandHandler(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    public async Task<ExecutionResult<TokenDto>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var accessToken = await _tokenService.RefreshAccessTokenAsync(request.AccessToken, request.RefreshToken, cancellationToken);

        return new ExecutionResult<TokenDto>(new TokenDto
        {
            AccessToken = accessToken,
            RefreshToken = _tokenService.CreateRefreshToken(accessToken)
        });
    }
}