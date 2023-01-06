using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Nmt.Core.Cache.Interfaces;
using Nmt.Infrastructure.Cache.Redis.Interfaces;

namespace Nmt.Core.Cache;

public class CachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<ICachePolicy<TRequest, TResponse>> _cachePolicies;
    private readonly IDistributedRedisCache _redisCache;
    private readonly ILogger<CachingBehavior<TRequest, TResponse>> _logger;

    public CachingBehavior(
        IEnumerable<ICachePolicy<TRequest, TResponse>> cachePolicies, 
        IDistributedRedisCache redisCache, 
        ILogger<CachingBehavior<TRequest, TResponse>> logger)
    {
        _cachePolicies = cachePolicies;
        _redisCache = redisCache;
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var cachePolicy = _cachePolicies.FirstOrDefault();
        if (cachePolicy == null)
        {
            return await next();
        }

        var cacheKey = cachePolicy.GetCacheKey(request);
        var cachedValue = await _redisCache.GetAsync<TResponse>(cacheKey, cancellationToken);

        if (cachedValue != null)
        {
            _logger.LogDebug($"Response retrieved {typeof(TRequest).FullName} from cache. CacheKey: {cacheKey}");
            return cachedValue;
        }

        var value = await next();
        var cacheEntryOptions = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = cachePolicy.AbsoluteExpirationRelativeToNow
        };

        await _redisCache.SetAsync(cacheKey, value, cacheEntryOptions, cancellationToken);

        _logger.LogDebug($"Cached response for {typeof(TRequest).FullName} with cache key: {cacheKey}");
        return value;
    }
}