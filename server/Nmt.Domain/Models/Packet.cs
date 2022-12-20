using System.Net.Sockets;
using Nmt.Domain.Enums;

namespace Nmt.Domain.Models;

public class Packet
{
    public Guid Id { get; set; }
    public Guid DeviceId { get; set; }
    public uint Ip { get; set; }
    public uint Size { get; set; }
    public ProtocolType Protocol { get; set; }
    public PacketStatus Status { get; set; }
}