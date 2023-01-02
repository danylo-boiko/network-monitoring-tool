using LS.Helpers.Hosting.API;
using MediatR;
using Nmt.Domain.Enums;

namespace Nmt.Core.CQRS.Commands.IpFilters.CreateIpFilter;

public record CreateIpFilterCommand : IRequest<ExecutionResult>
{
    public Guid UserId { get; set; }
    public uint Ip { get; set; }
    public IpFilterAction FilterAction { get; set; }
    public string? Comment { get; set; }
}