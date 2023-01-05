using LS.Helpers.Hosting.API;
using MediatR;

namespace Nmt.Core.CQRS.Queries.Users.GetUserById;

public record GetUserByIdQuery : IRequest<ExecutionResult<UserDto>>
{
    public Guid UserId { get; set; }

    public static string GetCacheKey(Guid userId)
    {
        return $"UserById_{userId}";
    }
}
