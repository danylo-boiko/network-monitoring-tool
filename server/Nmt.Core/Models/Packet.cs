using System.Net.Sockets;
using Nmt.Core.Enums;

namespace Nmt.Core.Models;

public class Packet
{
    public Guid Id { get; set; }
    public Guid DeviceId { get; set; }
    public uint Ip { get; set; }
    public uint Size { get; set; }
    public ProtocolType Protocol { get; set; }
    public PacketStatus Status { get; set; }
}