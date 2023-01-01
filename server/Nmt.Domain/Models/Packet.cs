using System.Net.Sockets;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Nmt.Domain.Enums;

namespace Nmt.Domain.Models;

public class Packet
{
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; }
    [BsonRepresentation(BsonType.String)]
    public Guid DeviceId { get; set; }
    [BsonRepresentation(BsonType.Int64)]
    public uint Ip { get; set; }
    [BsonRepresentation(BsonType.Int64)]
    public uint Size { get; set; }
    public ProtocolType Protocol { get; set; }
    public PacketStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
}