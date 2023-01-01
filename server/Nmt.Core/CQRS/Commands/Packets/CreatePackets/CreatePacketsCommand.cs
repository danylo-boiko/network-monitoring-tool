using MediatR;
using Nmt.Domain.Models;

namespace Nmt.Core.CQRS.Commands.Packets.CreatePackets;

public record CreatePacketsCommand : IRequest
{
    public IList<Packet> Packets { get; set; }
}