using Nmt.Domain.Common;

namespace Nmt.Core.CQRS.Queries.Users.GetUserWithDevicesAndIpFiltersById;

public class GetUserWithDevicesAndIpFiltersByIdCachePolicy : ICachePolicy<GetUserWithDevicesAndIpFiltersByIdQuery, UserDto>
{
    public string GetCacheKey(GetUserWithDevicesAndIpFiltersByIdQuery request)
    {
        return GetUserWithDevicesAndIpFiltersByIdQuery.GetCacheKey(request.UserId);
    }
}