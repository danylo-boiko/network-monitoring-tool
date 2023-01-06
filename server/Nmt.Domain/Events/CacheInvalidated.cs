using MediatR;

namespace Nmt.Domain.Events;

public record CacheInvalidated : INotification
{
    public string Key { get; set; }
}