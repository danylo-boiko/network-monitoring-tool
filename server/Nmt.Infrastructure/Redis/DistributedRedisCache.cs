using System.Text;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Nmt.Infrastructure.Redis.Interfaces;

namespace Nmt.Infrastructure.Redis;

public class DistributedRedisCache : IDistributedRedisCache
{
    private readonly IDistributedCache _distributedCache;

    public DistributedRedisCache(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken)
    {
        var bytesValue = await _distributedCache.GetAsync(key, cancellationToken);

        if (bytesValue == null)
        {
            return default;
        }

        try
        {
            var jsonString = Encoding.UTF8.GetString(bytesValue);

            return JsonConvert.DeserializeObject<T>(jsonString, new JsonSerializerSettings
            {
                MissingMemberHandling = MissingMemberHandling.Error
            });
        }
        catch (JsonSerializationException)
        {
            await _distributedCache.RemoveAsync(key, cancellationToken);
            return default;
        }
    }

    public async Task SetAsync<T>(string key, T value, DistributedCacheEntryOptions options, CancellationToken cancellationToken)
    {
        var jsonString = JsonConvert.SerializeObject(value);
        var bytesValue = Encoding.UTF8.GetBytes(jsonString);

        await _distributedCache.SetAsync(key, bytesValue, options, cancellationToken);
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken)
    {
        await _distributedCache.RemoveAsync(key, cancellationToken);
    }

    public async Task<bool> ContainsKey(string key, CancellationToken cancellationToken)
    {
        return await _distributedCache.GetAsync(key, cancellationToken) != null;
    }
}