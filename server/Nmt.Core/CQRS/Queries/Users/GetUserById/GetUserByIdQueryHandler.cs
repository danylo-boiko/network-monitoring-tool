using MediatR;
using Microsoft.EntityFrameworkCore;
using Nmt.Infrastructure.Data.Postgres;

namespace Nmt.Core.CQRS.Queries.Users.GetUserById;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto>
{
    private readonly PostgresDbContext _dbContext;

    public GetUserByIdQueryHandler(PostgresDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
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
            throw new ArgumentException($"User with id '{request.UserId}' not found");
        }

        return userDto;
    }
} 
