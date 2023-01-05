using MediatR;

namespace Nmt.Domain.Events;

public record CacheInvalidated : INotification
{
    public string Key { get; }

    public CacheInvalidated(string key)
    {
        Key = key;
    }
}