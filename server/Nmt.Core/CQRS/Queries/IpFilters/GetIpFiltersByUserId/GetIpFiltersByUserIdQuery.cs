using MediatR;

namespace Nmt.Core.CQRS.Queries.IpFilters.GetIpFiltersByUserId;

public record GetIpFiltersByUserIdQuery : IRequest<IList<IpFilterDto>>
{
    public Guid UserId { get; set; }
}