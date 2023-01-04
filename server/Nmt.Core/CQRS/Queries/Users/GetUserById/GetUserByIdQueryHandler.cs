using LS.Helpers.Hosting.API;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nmt.Domain.Consts;
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
            where user.Id == request.UserId
            select new UserDto
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email,
                Devices = _dbContext.Devices.Where(d => d.UserId == user.Id).Select(d => new UserDto.UserDeviceDto
                {
                    Id = d.Id,
                    Hostname = d.Hostname,
                    MachineSpecificStamp = d.MachineSpecificStamp,
                    CreatedAt = d.CreatedAt
                }).ToList(),
                IpFilters = _dbContext.IpFilters.Where(i => i.UserId == user.Id).Select(i => new UserDto.IpFilterDto
                {
                    Id = i.Id,
                    Ip = i.Ip,
                    FilterAction = i.FilterAction,
                    Comment = i.Comment,
                    CreatedAt = i.CreatedAt
                }).ToList()
            };

        var userDto = await userQuery.FirstOrDefaultAsync(cancellationToken);

        if (userDto == null)
        {
            return new ExecutionResult<UserDto>(new ErrorInfo(StatusCodes.NotFound, $"User with id '{request.UserId}' not found"));
        }

        return new ExecutionResult<UserDto>(userDto);
    }
} 
