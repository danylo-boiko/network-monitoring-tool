using LS.Helpers.Hosting.API;
using Nmt.Core.Cache.Interfaces;

namespace Nmt.Core.CQRS.Queries.IpFilters.GetIpFiltersByUserId;

public class GetIpFiltersByUserIdCachePolicy : ICachePolicy<GetIpFiltersByUserIdQuery, ExecutionResult<IList<IpFilterDto>>>
{
    public string GetCacheKey(GetIpFiltersByUserIdQuery request)
    {
        return GetIpFiltersByUserIdQuery.GetCacheKey(request.UserId);
    }
}