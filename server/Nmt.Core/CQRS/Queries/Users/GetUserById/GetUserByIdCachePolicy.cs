using LS.Helpers.Hosting.API;
using Nmt.Core.Cache.Interfaces;

namespace Nmt.Core.CQRS.Queries.Users.GetUserById;

public class GetUserByIdCachePolicy : ICachePolicy<GetUserByIdQuery, ExecutionResult<UserDto>>
{
    public string GetCacheKey(GetUserByIdQuery request)
    {
        return GetUserByIdQuery.GetCacheKey(request.UserId);
    }
}