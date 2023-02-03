using LS.Helpers.Hosting.API;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nmt.Domain.Consts;
using Nmt.Domain.Exceptions;
using Nmt.Infrastructure.Data.Postgres;

namespace Nmt.Core.CQRS.Queries.Users.GetUserWithDevicesAndIpFiltersById;

public class GetUserWithDevicesAndIpFiltersByIdQueryHandler : IRequestHandler<GetUserWithDevicesAndIpFiltersByIdQuery, UserDto>
{
    private readonly PostgresDbContext _dbContext;

    public GetUserWithDevicesAndIpFiltersByIdQueryHandler(PostgresDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<UserDto> Handle(GetUserWithDevicesAndIpFiltersByIdQuery request, CancellationToken cancellationToken)
    {
        var userQuery =
            from user in _dbContext.Users
            where user.Id == request.UserId
            select new UserDto
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email,
                Devices = _dbContext.Devices.Where(d => d.UserId == user.Id).Select(d => new UserDto.DeviceDto
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
            throw new DomainException( $"User with id '{request.UserId}' not found")
            {
                Code = ExceptionCodes.NotFound,
                Property = nameof(request.UserId)
            };
        }

        return userDto;
    }
} 
