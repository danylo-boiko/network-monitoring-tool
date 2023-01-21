using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nmt.Core.CQRS.Queries.Users.GetUserWithDevicesAndIpFiltersById;
using Nmt.Domain.BusEvents;
using Nmt.Domain.Enums;
using Nmt.Domain.Events;
using Nmt.Domain.Models;
using Nmt.Infrastructure.Data.Postgres;

namespace Nmt.Core.BusEventHandlers;

public class BlockIpAddressesEventHandler : IConsumer<BlockIpAddressesEvent>
{
    private readonly PostgresDbContext _dbContext;
    private readonly IMediator _mediator;

    public BlockIpAddressesEventHandler(PostgresDbContext dbContext, IMediator mediator)
    {
        _dbContext = dbContext;
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<BlockIpAddressesEvent> context)
    {
        var userIdQuery =
            from device in _dbContext.Devices
            join user in _dbContext.Users on device.UserId equals user.Id
            where device.Id == context.Message.DeviceId
            select user.Id;

        var userId = await userIdQuery.FirstAsync(context.CancellationToken);

        var dropIpFilters = context.Message.Ips.Select(ip => new IpFilter
        {
            UserId = userId,
            Ip = ip,
            FilterAction = IpFilterAction.Drop,
            Comment = "Created automatically",
            CreatedAt = DateTime.UtcNow
        });

        await _dbContext.IpFilters.AddRangeAsync(dropIpFilters, context.CancellationToken);

        await _mediator.Publish(new CacheInvalidated
        {
            Key = GetUserWithDevicesAndIpFiltersByIdQuery.GetCacheKey(userId)
        }, context.CancellationToken);
    }
}