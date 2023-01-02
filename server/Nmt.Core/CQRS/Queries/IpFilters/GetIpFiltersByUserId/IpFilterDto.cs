using Nmt.Domain.Enums;

namespace Nmt.Core.CQRS.Queries.IpFilters.GetIpFiltersByUserId;

public class IpFilterDto
{
    public Guid Id { get; set; }
    public uint Ip { get; set; }
    public IpFilterAction FilterAction { get; set; }
    public DateTime CreatedAt { get; set; }
}