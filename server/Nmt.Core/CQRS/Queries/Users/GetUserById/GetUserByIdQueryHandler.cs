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
        var userQuery =
            from user in _dbContext.Users
            join device in _dbContext.Devices on user.Id equals device.UserId into devices
            where user.Id == request.UserId
            select new UserDto()
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email,
                Devices = devices.Select(device => new UserDto.UserDeviceDto
                {
                    Id = device.Id,
                    Hostname = device.Hostname,
                    MachineSpecificStamp = device.MachineSpecificStamp,
                    CreatedAt = device.CreatedAt
                }).ToList()
            };

        var userDto = await userQuery.FirstOrDefaultAsync(cancellationToken);

        if (userDto == null)
        {
            return new ExecutionResult<UserDto>(new ErrorInfo($"User with id '{request.UserId}' not found"));
        }

        return new ExecutionResult<UserDto>(userDto);
    }
} 
