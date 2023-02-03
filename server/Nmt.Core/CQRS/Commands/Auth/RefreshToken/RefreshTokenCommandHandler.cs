using MediatR;
using Nmt.Core.CQRS.Commands.Auth.Login;
using Nmt.Core.Services.Interfaces;

namespace Nmt.Core.CQRS.Commands.Auth.RefreshToken;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, TokenDto>
{
    private readonly ITokensService _tokensService;

    public RefreshTokenCommandHandler(ITokensService tokensService)
    {
        _tokensService = tokensService;
    }

    public async Task<TokenDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var accessToken = await _tokensService.RefreshAccessTokenAsync(request.AccessToken, request.RefreshToken, cancellationToken);

        return new TokenDto
        {
            AccessToken = accessToken,
            RefreshToken = _tokensService.CreateRefreshToken(accessToken)
        };
    }
}