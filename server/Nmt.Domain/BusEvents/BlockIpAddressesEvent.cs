namespace Nmt.Domain.BusEvents;

public record BlockIpAddressesEvent : BaseEvent
{
    public Guid DeviceId { get; set; }
    public IList<uint> Ips { get; set; }
}