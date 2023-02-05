using MediatR;
using Nmt.Domain.Enums;

namespace Nmt.Core.CQRS.Queries.Packets.GetPacketsChartDataByDeviceId;

public record GetPacketsChartDataByDeviceIdQuery : IRequest<PacketsChartDataDto>
{
    public Guid DeviceId { get; set; }
    public DateRangeMode DateRangeMode { get; set; }
}