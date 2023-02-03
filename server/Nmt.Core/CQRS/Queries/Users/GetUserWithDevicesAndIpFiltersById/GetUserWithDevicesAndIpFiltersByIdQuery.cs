using MediatR;

namespace Nmt.Core.CQRS.Queries.Users.GetUserWithDevicesAndIpFiltersById;

public record GetUserWithDevicesAndIpFiltersByIdQuery : IRequest<UserDto>
{
    public Guid UserId { get; set; }

    public static string GetCacheKey(Guid userId)
    {
        return $"UserWithDevicesAndIpFiltersById_{userId}";
    }
}
