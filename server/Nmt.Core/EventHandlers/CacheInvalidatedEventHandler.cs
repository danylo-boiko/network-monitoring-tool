using MediatR;
using Nmt.Domain.Events;
using Nmt.Infrastructure.Redis.Interfaces;

namespace Nmt.Core.EventHandlers;

public class CacheInvalidatedEventHandler : INotificationHandler<CacheInvalidated>
{
    private readonly IDistributedRedisCache _redisCache;

    public CacheInvalidatedEventHandler(IDistributedRedisCache redisCache)
    {
        _redisCache = redisCache;
    }

    public async Task Handle(CacheInvalidated notification, CancellationToken cancellationToken)
    {
        await _redisCache.RemoveAsync(notification.Key, cancellationToken);
    }
}