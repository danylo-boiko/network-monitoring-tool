using HotChocolate.AspNetCore.Authorization;
using Nmt.Core.CQRS.Queries.Packets.GetPacketsByDeviceId;
using Nmt.GraphQL.Services.Interfaces;

namespace Nmt.GraphQL.Queries;

[Authorize]
[ExtendObjectType(ObjectTypes.Query)]
public class Packets
{
    public async Task<IList<PacketDto>> GetPacketsByDeviceId([Service] IExecutionResultService executionResultService, GetPacketsByDeviceIdQuery input)
    {
        return await executionResultService.HandleExecutionResultRequest(input);
    }
}