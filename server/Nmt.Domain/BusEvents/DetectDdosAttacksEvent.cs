namespace Nmt.Domain.BusEvents;

public record DetectDdosAttacksEvent : BaseEvent
{
    public Guid DeviceId { get; set; }
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
}