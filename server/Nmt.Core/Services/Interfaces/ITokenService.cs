using Nmt.Domain.Common;

namespace Nmt.Core.Services.Interfaces;

public interface ITokenService : IService
{
    public Task<string> CreateAccessTokenAsync(Guid userId, Guid? deviceId, CancellationToken cancellationToken);
    public Task<string> RefreshAccessTokenAsync(string accessToken, string refreshToken, CancellationToken cancellationToken);
    public string CreateRefreshToken(string accessToken);
}