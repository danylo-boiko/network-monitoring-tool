using LS.Helpers.Hosting.API;
using MediatR;

namespace Nmt.Core.CQRS.Commands.IpFilters.DeleteIpFilter;

public record DeleteIpFilterCommand : IRequest<ExecutionResult<bool>>
{
    public Guid IpFilterId { get; set; }
}