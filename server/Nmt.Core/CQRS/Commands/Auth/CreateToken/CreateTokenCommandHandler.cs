using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Nmt.Domain.Configs;
using Nmt.Domain.Consts;
using Nmt.Infrastructure.Data.Postgres;

namespace Nmt.Core.CQRS.Commands.Auth.CreateToken;

public class CreateTokenCommandHandler : IRequestHandler<CreateTokenCommand, string>
{
    private readonly PostgresDbContext _dbContext;
    private readonly JwtConfig _jwtConfig;

    public CreateTokenCommandHandler(PostgresDbContext dbContext, JwtConfig jwtConfig)
    {
        _dbContext = dbContext;
        _jwtConfig = jwtConfig;
    }

    public async Task<string> Handle(CreateTokenCommand request, CancellationToken cancellationToken)
    {
        var signingCredentials = GetSigningCredentials();
        var claims = await GetClaims(request.UserId, request.DeviceId, cancellationToken);

        var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
        return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
    }

    private SigningCredentials GetSigningCredentials()
    {
        var key = Encoding.UTF8.GetBytes(_jwtConfig.Secret);
        var secret = new SymmetricSecurityKey(key);

        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
    }

    private async Task<IList<Claim>> GetClaims(Guid userId, Guid? deviceId, CancellationToken cancellationToken)
    {
        var claims = new List<Claim>
        {
            new (AuthClaims.UserId, userId.ToString()),
            new (AuthClaims.DeviceId, deviceId?.ToString() ?? string.Empty)
        };

        var rolesQuery =
            from userRole in _dbContext.UserRoles
            join role in _dbContext.Roles on userRole.RoleId equals role.Id
            join roleClaim in _dbContext.RoleClaims on role.Id equals roleClaim.RoleId into roleClaims
            where userRole.UserId == userId
            select new
            {
                RoleName = role.Name,
                Claims = roleClaims.Select(c => c.ClaimValue).ToList()
            };

        var roles = await rolesQuery.ToListAsync(cancellationToken);
        var uniqueRoleClaims = roles.SelectMany(role => role.Claims).Distinct().ToList();

        claims.AddRange(roles.Select(role => new Claim(AuthClaims.Role, role.RoleName)));
        claims.AddRange(uniqueRoleClaims.Select(roleClaim => new Claim(AuthClaims.RoleClaim, roleClaim)));

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
