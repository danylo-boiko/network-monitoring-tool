using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Nmt.Domain.Configs;
using Nmt.Domain.Consts;
using Nmt.Domain.Models;

namespace Nmt.Core.Commands.Auth;

public record CreateTokenCommand : IRequest<string>
{
    public User User { get; set; }
    public Guid? DeviceId { get; set; }
}

public class CreateTokenCommandHandler : IRequestHandler<CreateTokenCommand, string>
{
    private readonly UserManager<User> _userManager;
    private readonly JwtConfig _jwtConfig;

    public CreateTokenCommandHandler(UserManager<User> userManager, JwtConfig jwtConfig)
    {
        _userManager = userManager;
        _jwtConfig = jwtConfig;
    }

    public async Task<string> Handle(CreateTokenCommand request, CancellationToken cancellationToken)
    {
        var signingCredentials = GetSigningCredentials();
        var claims = await GetClaims(request.User, request.DeviceId);

        var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
        return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
    }

    private SigningCredentials GetSigningCredentials()
    {
        var key = Encoding.UTF8.GetBytes(_jwtConfig.Secret);
        var secret = new SymmetricSecurityKey(key);
        
        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
    }

    private async Task<IList<Claim>> GetClaims(User user, Guid? deviceId)
    {
        var claims = new List<Claim>
        {
            new (AuthClaims.UserId, user.Id.ToString()),
            new (AuthClaims.DeviceId, deviceId?.ToString() ?? string.Empty)
        };

        var roles = await _userManager.GetRolesAsync(user);

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        return claims;
    }

    private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, IList<Claim> claims)
    {
        return new JwtSecurityToken
        (
            issuer: _jwtConfig.ValidIssuer,
            claims: claims,
            expires: DateTime.UtcNow.AddDays(_jwtConfig.ExpiresIn),
            signingCredentials: signingCredentials
        );
    }
}
