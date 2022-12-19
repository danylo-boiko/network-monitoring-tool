using System.Net.Sockets;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Nmt.Core.Enums;
using Nmt.Core.Models;
using Nmt.Grpc.Protos;

namespace Nmt.Grpc.Services;

public class PacketsService : Packets.PacketsBase
{
    private readonly ILogger<PacketsService> _logger;

    public PacketsService(ILogger<PacketsService> logger)
    {
        _logger = logger;
    }

    public override async Task<Empty> AddPackets(AddPacketsRequest request, ServerCallContext context)
    {
        var castedModels = request.Packets.Select(r => new Packet
        {
            Ip = r.Ip,
            Size = r.Size,
            Protocol = (ProtocolType)r.Protocol,
            Status = (PacketStatus)r.Status
        }).ToList();

        foreach (var packet in castedModels)
        {
            _logger.LogInformation($"Ip: {packet.Ip}, size: {packet.Size}, status: {packet.Status}, protocol: {packet.Protocol}");
        }

        return new Empty();
    }
}