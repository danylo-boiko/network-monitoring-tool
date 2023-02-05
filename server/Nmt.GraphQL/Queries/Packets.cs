using MediatR;
using Nmt.Core.CQRS.Queries.Packets.GetPacketsChartDataByDeviceId;
using Nmt.Domain.Enums;
using Nmt.GraphQL.Attributes;
using Nmt.GraphQL.Consts;

namespace Nmt.GraphQL.Queries;

[ExtendObjectType(ObjectTypes.Query)]
public class Packets
{
    [PermissionsAuthorize(Permission.PacketsRead)]
    public async Task<PacketsChartDataDto> GetPacketsChartDataByDeviceId([Service] IMediator mediator, GetPacketsChartDataByDeviceIdQuery input)
    {
        return await mediator.Send(input);
    }
}