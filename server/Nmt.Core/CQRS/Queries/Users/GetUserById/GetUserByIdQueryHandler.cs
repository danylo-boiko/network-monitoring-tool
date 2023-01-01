using LS.Helpers.Hosting.API;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nmt.Infrastructure.Data.Postgres;

namespace Nmt.Core.CQRS.Queries.Users.GetUserById;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, ExecutionResult<UserDto>>
{
    private readonly PostgresDbContext _dbContext;

    public GetUserByIdQueryHandler(PostgresDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ExecutionResult<UserDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var userDto = await _dbContext.Users
            .Where(u => u.Id == request.UserId)
            .Select(u => new UserDto
            {
                Id = u.Id,
                Username = u.UserName!,
                Email = u.Email!
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (userDto == null)
        {
            return new ExecutionResult<UserDto>(new ErrorInfo($"User with id '{request.UserId}' not found"));
        }

        return new ExecutionResult<UserDto>(userDto);
    }
} 
