using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Nmt.Core.Extensions;
using Nmt.Core.Services.Interfaces;
using Nmt.Domain.Configs;
using Nmt.Domain.Consts;
using Nmt.Infrastructure.Data.Postgres;

namespace Nmt.Core.Services;

public class TokensService : ITokensService
{
    private readonly PostgresDbContext _dbContext;
    private readonly JwtConfig _jwtConfig;

    public TokensService(PostgresDbContext dbContext, JwtConfig jwtConfig)
    {
        _dbContext = dbContext;
        _jwtConfig = jwtConfig;
    }

    public async Task<string> CreateAccessTokenAsync(Guid userId, Guid? deviceId, CancellationToken cancellationToken)
    {
        var signingCredentials = GetSigningCredentials();
        var claims = await GetAccessTokenClaims(userId, deviceId, cancellationToken);
        var expireDate = DateTime.UtcNow.AddHours(_jwtConfig.AccessTokenExpiresInHours);

        var tokenOptions = GenerateTokenOptions(signingCredentials, claims, expireDate);
        return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
    }

    public async Task<string> RefreshAccessTokenAsync(string accessToken, string refreshToken, CancellationToken cancellationToken)
    {
        var accessTokenClaims = GetClaimsPrincipalFromToken(accessToken);
        var refreshTokenClaims = GetClaimsPrincipalFromToken(refreshToken);
        var accessTokenUserId = accessTokenClaims.FindFirstGuid(AuthClaims.UserId);

        if (accessTokenUserId != refreshTokenClaims.FindFirstGuid(AuthClaims.UserId))
        {
            throw new SecurityTokenException("Access and refresh tokes have user id mismatch");
        }

        if (ComputeSecurityHash(accessTokenUserId, accessToken) != refreshTokenClaims.FindFirstValue(AuthClaims.SecurityHash))
        {
            throw new SecurityTokenException("Invalid security hash");
        }

        var accessTokenDeviceId = accessTokenClaims.FindFirstGuid(AuthClaims.DeviceId);

        return await CreateAccessTokenAsync(accessTokenUserId, accessTokenDeviceId, cancellationToken);
    }

    public string CreateRefreshToken(string accessToken)
    {
        var signingCredentials = GetSigningCredentials();
        var claims = GetRefreshTokenClaims(accessToken);
        var expireDate = DateTime.UtcNow.AddDays(_jwtConfig.RefreshTokenExpiresInDays);

        var tokenOptions = GenerateTokenOptions(signingCredentials, claims, expireDate);
        return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
    }

    private SigningCredentials GetSigningCredentials()
    {
        var key = Encoding.UTF8.GetBytes(_jwtConfig.Secret);
        var secret = new SymmetricSecurityKey(key);

        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
    }

    private async Task<IList<Claim>> GetAccessTokenClaims(Guid userId, Guid? deviceId, CancellationToken cancellationToken)
    {
        var claims = new List<Claim>
        {
            new (AuthClaims.UserId, userId.ToString()),
            new (AuthClaims.DeviceId, deviceId != null && deviceId.Value != Guid.Empty 
                ? deviceId.Value.ToString() 
                : string.Empty)
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

    private IList<Claim> GetRefreshTokenClaims(string accessToken)
    {
        var claims = GetClaimsPrincipalFromToken(accessToken);
        var userId = claims.FindFirstGuid(AuthClaims.UserId);

        return new List<Claim>
        {
            new (AuthClaims.UserId, userId.ToString()),
            new (AuthClaims.SecurityHash, ComputeSecurityHash(userId, accessToken))
        };
    }

    private string ComputeSecurityHash(Guid userId, string accessToken)
    {
        using var sha256 = SHA256.Create();

        var bytes = Encoding.UTF8.GetBytes($"{userId}{accessToken}{_jwtConfig.Secret}");
        var computedHash = sha256.ComputeHash(bytes);

        return Convert.ToHexString(computedHash).ToLower();
    }

    private ClaimsPrincipal GetClaimsPrincipalFromToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            ValidateAudience = false,
            ValidIssuer = _jwtConfig.ValidIssuer,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.Secret)),
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token");
        }

        return principal;
    }

    private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, IList<Claim> claims, DateTime expireDate)
    {
        return new JwtSecurityToken
        (
            issuer: _jwtConfig.ValidIssuer,
            claims: claims,
            expires: expireDate,
            signingCredentials: signingCredentials
        );
    }
}