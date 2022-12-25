using System.Net.Sockets;
using System.Security.Claims;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using MongoDB.Driver;
using Nmt.Domain.Consts;
using Nmt.Domain.Enums;
using Nmt.Domain.Models;
using Nmt.Grpc.Protos;
using Nmt.Infrastructure.Data.Mongo;

namespace Nmt.Grpc.Services;

[Authorize]
public class PacketsService : Packets.PacketsBase
{
    private readonly IMongoCollection<Packet> _packetsCollection;

    public PacketsService(IMongoDatabase mongoDatabase, MongoDbSettings mongoDbSettings)
    {
        _packetsCollection = mongoDatabase.GetCollection<Packet>(mongoDbSettings.PacketsCollectionName);
    }

    public override async Task<Empty> AddPackets(AddPacketsRequest request, ServerCallContext context)
    {
        var deviceId = context.GetHttpContext().User.FindFirstValue(AuthClaims.DeviceId);

        var packets = request.Packets.Select(pm => new Packet
        {
            DeviceId = Guid.Parse(deviceId!),
            Ip = pm.Ip,
            Size = pm.Size,
            Protocol = (ProtocolType)pm.Protocol,
            Status = (PacketStatus)pm.Status
        });

        await _packetsCollection.InsertManyAsync(packets, cancellationToken: context.CancellationToken);

        return new Empty();
    }
}