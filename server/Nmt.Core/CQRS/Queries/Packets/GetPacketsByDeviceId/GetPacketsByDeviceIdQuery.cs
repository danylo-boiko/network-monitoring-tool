using LS.Helpers.Hosting.API;
using MediatR;

namespace Nmt.Core.CQRS.Queries.Packets.GetPacketsByDeviceId;

public record GetPacketsByDeviceIdQuery : IRequest<ExecutionResult<IList<PacketDto>>>
{
    public Guid DeviceId { get; set; }
    public DateTime? DateFrom { get; set; }
    public DateTime? DateTo { get; set; }
}