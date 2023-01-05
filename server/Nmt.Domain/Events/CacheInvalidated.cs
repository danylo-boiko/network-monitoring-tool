using MediatR;

namespace Nmt.Domain.Events;

public record CacheInvalidated(string Key) : INotification;