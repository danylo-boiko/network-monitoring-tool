using System.Net.Sockets;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;
using Nmt.Core.CQRS.Commands.Packets.CreatePackets;
using Nmt.Domain.Consts;
using Nmt.Domain.Enums;
using Nmt.Domain.Models;
using Nmt.Grpc.Attributes;
using Nmt.Grpc.Extensions;
using Nmt.Grpc.Protos;

namespace Nmt.Grpc.Services;

public class PacketsService : Packets.PacketsBase
{
    private readonly IMediator _mediator;

    public PacketsService(IMediator mediator)
    {
        _mediator = mediator;
    }

    [PermissionsAuthorize(Permission.PacketsCreate)]
    public override async Task<Empty> AddPackets(AddPacketsRequest request, ServerCallContext context)
    {
        var deviceId = context.GetAuthClaimValue(AuthClaims.DeviceId);

        await _mediator.Send(new CreatePacketsCommand
        {
            DeviceId = deviceId,
            Packets = request.Packets.Select(pm => new Packet
            {
                DeviceId = deviceId,
                Ip = pm.Ip,
                Size = pm.Size,
                Protocol = (ProtocolType)pm.Protocol,
                Status = (PacketStatus)pm.Status,
                CreatedAt = pm.CreatedAt.ToDateTime()
            }).ToList()
        }, context.CancellationToken);

        return new Empty();
    }
}