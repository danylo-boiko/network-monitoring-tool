using LS.Helpers.Hosting.API;
using MediatR;
using Nmt.Domain.Enums;

namespace Nmt.Core.CQRS.Commands.IpFilters.UpdateIpFilter;

public record UpdateIpFilterCommand : IRequest<ExecutionResult<bool>>
{
    public Guid IpFilterId { get; set; }
    public IpFilterAction FilterAction { get; set; }
    public string? Comment { get; set; }
}