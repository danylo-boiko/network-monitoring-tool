using System.Net.Sockets;
using Nmt.Domain.Enums;

namespace Nmt.Core.CQRS.Queries.Packets.GetPacketsByDeviceId;

public class PacketDto
{
    public Guid Id { get; set; }
    public long Ip { get; set; }
    public long Size { get; set; }
    public ProtocolType Protocol { get; set; }
    public PacketStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
}