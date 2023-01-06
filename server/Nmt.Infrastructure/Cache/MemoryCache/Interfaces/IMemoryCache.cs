namespace Nmt.Infrastructure.Cache.MemoryCache.Interfaces;

public interface IMemoryCache
{
    public T? Get<T>(string key);
    public void Set<T>(string key, T value, TimeSpan duration);
}