using MediatR;
using Nmt.Core.CQRS.Queries.Packets.GetPacketsByDeviceId;
using Nmt.Domain.Enums;
using Nmt.GraphQL.Attributes;
using Nmt.GraphQL.Consts;

namespace Nmt.GraphQL.Queries;

[ExtendObjectType(ObjectTypes.Query)]
public class Packets
{
    [PermissionsAuthorize(Permission.PacketsRead)]
    public async Task<IList<PacketDto>> GetPacketsByDeviceId([Service] IMediator mediator, GetPacketsByDeviceIdQuery input)
    {
        return await mediator.Send(input);
    }
}