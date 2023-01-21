namespace Nmt.Domain.BusEvents;

public abstract record BaseEvent
{
    public Guid Id { get; } = Guid.NewGuid();
    public DateTime CreatedAt { get; } = DateTime.UtcNow;
}