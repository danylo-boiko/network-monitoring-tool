using HotChocolate.AspNetCore.Authorization;
using MediatR;
using Nmt.Core.CQRS.Queries.Packets.GetPacketsByDeviceId;
using Nmt.Core.Extensions;

namespace Nmt.GraphQL.Queries;

[Authorize]
[ExtendObjectType(ObjectTypes.Query)]
public class Packets
{
    public async Task<IList<PacketDto>> GetPacketsByDeviceId([Service] IMediator mediator, GetPacketsByDeviceIdQuery input)
    {
        var packersResult = await mediator.Send(input);

        if (!packersResult.Success)
        {
            throw packersResult.ToGraphQLException();
        }

        return packersResult.Value;
    }
}