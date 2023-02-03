using MediatR;

namespace Nmt.Core.CQRS.Commands.IpFilters.DeleteIpFilter;

public record DeleteIpFilterCommand : IRequest<bool>
{
    public Guid IpFilterId { get; set; }
}