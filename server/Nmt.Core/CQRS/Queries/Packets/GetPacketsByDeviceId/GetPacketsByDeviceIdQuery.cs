using MediatR;

namespace Nmt.Core.CQRS.Queries.Packets.GetPacketsByDeviceId;

public record GetPacketsByDeviceIdQuery : IRequest<IList<PacketDto>>
{
    public Guid DeviceId { get; set; }
    public DateTime? DateFrom { get; set; }
    public DateTime? DateTo { get; set; }
}