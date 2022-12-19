using System.Net.Sockets;
using Nmt.Core.Enums;

namespace Nmt.Core.Models;

public class Packet
{
    public uint Ip { get; set; }
    public uint Size { get; set; }
    public ProtocolType Protocol { get; set; }
    public PacketStatus Status { get; set; }
}