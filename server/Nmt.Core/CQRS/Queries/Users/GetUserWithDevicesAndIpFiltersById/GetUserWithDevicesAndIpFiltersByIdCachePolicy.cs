using LS.Helpers.Hosting.API;
using Nmt.Core.Cache.Interfaces;

namespace Nmt.Core.CQRS.Queries.Users.GetUserWithDevicesAndIpFiltersById;

public class GetUserWithDevicesAndIpFiltersByIdCachePolicy : ICachePolicy<GetUserWithDevicesAndIpFiltersByIdQuery, ExecutionResult<UserDto>>
{
    public string GetCacheKey(GetUserWithDevicesAndIpFiltersByIdQuery request)
    {
        return GetUserWithDevicesAndIpFiltersByIdQuery.GetCacheKey(request.UserId);
    }
}