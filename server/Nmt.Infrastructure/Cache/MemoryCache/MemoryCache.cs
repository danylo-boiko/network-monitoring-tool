using Microsoft.Extensions.Caching.Memory;

namespace Nmt.Infrastructure.Cache.MemoryCache;

public class MemoryCache : Interfaces.IMemoryCache
{
    private readonly IMemoryCache _cache;

    public MemoryCache(IMemoryCache cache)
    {
        _cache = cache;
    }

    public T? Get<T>(string key)
    {
        return _cache.Get<T>(key);
    }

    public void Set<T>(string key, T value, DateTimeOffset duration)
    {
        _cache.Set(key, value, duration);
    }

    public void Remove(string key)
    {
        _cache.Remove(key);
    }
}