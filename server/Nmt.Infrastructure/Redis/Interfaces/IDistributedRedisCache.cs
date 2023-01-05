using Microsoft.Extensions.Caching.Distributed;

namespace Nmt.Infrastructure.Redis.Interfaces;

public interface IDistributedRedisCache
{
    public Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken);
    public Task SetAsync<T>(string key, T value, DistributedCacheEntryOptions options, CancellationToken cancellationToken);
    public Task RemoveAsync(string key, CancellationToken cancellationToken);
    public Task<bool> ContainsKey(string key, CancellationToken cancellationToken);
}