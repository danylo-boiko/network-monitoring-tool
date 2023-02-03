using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Nmt.Domain.Common;
using Nmt.Infrastructure.Cache.Redis.Interfaces;

namespace Nmt.Core.Behaviors;

public class CachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<ICachePolicy<TRequest, TResponse>> _cachePolicies;
    private readonly IDistributedRedisCache _redisCache;

    public CachingBehavior(IEnumerable<ICachePolicy<TRequest, TResponse>> cachePolicies, IDistributedRedisCache redisCache)
    {
        _cachePolicies = cachePolicies;
        _redisCache = redisCache;
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
            return cachedValue;
        }

        var value = await next();
        var cacheEntryOptions = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = cachePolicy.AbsoluteExpirationRelativeToNow
        };

        await _redisCache.SetAsync(cacheKey, value, cacheEntryOptions, cancellationToken);

        return value;
    }
}