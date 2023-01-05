using LS.Helpers.Hosting.API;
using MediatR;

namespace Nmt.Core.CQRS.Queries.IpFilters.GetIpFiltersByUserId;

public record GetIpFiltersByUserIdQuery : IRequest<ExecutionResult<IList<IpFilterDto>>>
{
    public Guid UserId { get; set; }

    public static string GetCacheKey(Guid userId)
    {
        return $"IpFiltersByUserId_{userId}";
    }
}