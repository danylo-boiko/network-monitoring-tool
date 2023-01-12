using Nmt.Core.CQRS.Queries.Packets.GetPacketsByDeviceId;
using Nmt.Domain.Enums;
using Nmt.GraphQL.Attributes;
using Nmt.GraphQL.Consts;
using Nmt.GraphQL.Services.Interfaces;

namespace Nmt.GraphQL.Queries;

[ExtendObjectType(ObjectTypes.Query)]
public class Packets
{
    [PermissionsAuthorize(Permission.PacketsRead)]
    public async Task<IList<PacketDto>> GetPacketsByDeviceId([Service] IExecutionResultService executionResultService, GetPacketsByDeviceIdQuery input)
    {
        return await executionResultService.HandleExecutionResultRequestAsync(input);
    }
}