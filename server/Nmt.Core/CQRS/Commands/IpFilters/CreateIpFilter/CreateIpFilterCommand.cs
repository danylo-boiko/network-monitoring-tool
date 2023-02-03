using MediatR;
using Nmt.Domain.Enums;

namespace Nmt.Core.CQRS.Commands.IpFilters.CreateIpFilter;

public record CreateIpFilterCommand : IRequest<Guid>
{
    public Guid? UserId { get; set; }
    public long Ip { get; set; }
    public IpFilterAction FilterAction { get; set; }
    public string? Comment { get; set; }
}