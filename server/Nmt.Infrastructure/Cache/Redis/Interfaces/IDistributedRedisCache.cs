using Microsoft.Extensions.Caching.Distributed;

namespace Nmt.Infrastructure.Cache.Redis.Interfaces;

public interface IDistributedRedisCache
{
    public Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken);
    public Task SetAsync<T>(string key, T value, DistributedCacheEntryOptions options, CancellationToken cancellationToken);
    public Task RemoveAsync(string key, CancellationToken cancellationToken);
}