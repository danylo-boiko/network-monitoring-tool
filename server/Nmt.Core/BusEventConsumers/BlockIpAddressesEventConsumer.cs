using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nmt.Core.CQRS.Queries.Users.GetUserWithDevicesAndIpFiltersById;
using Nmt.Domain.BusEvents;
using Nmt.Domain.Enums;
using Nmt.Domain.Events;
using Nmt.Domain.Models;
using Nmt.Infrastructure.Data.Postgres;

namespace Nmt.Core.BusEventConsumers;

public class BlockIpAddressesEventConsumer : IConsumer<BlockIpAddressesEvent>
{
    private readonly PostgresDbContext _dbContext;
    private readonly IMediator _mediator;

    public BlockIpAddressesEventConsumer(PostgresDbContext dbContext, IMediator mediator)
    {
        _dbContext = dbContext;
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<BlockIpAddressesEvent> context)
    {
        var userId = await _dbContext.Devices
            .Where(d => d.Id == context.Message.DeviceId)
            .Select(d => d.UserId)
            .FirstAsync(context.CancellationToken);

        var alreadyExistsIpFilters = (await _dbContext.IpFilters
            .Where(i => i.UserId == userId && context.Message.Ips.Contains(i.Ip))
            .Select(i => i.Ip)
            .ToListAsync(context.CancellationToken))
            .ToHashSet();

        var dropIpFilters = context.Message.Ips
            .Where(ip => !alreadyExistsIpFilters.Contains(ip))
            .Select(ip => new IpFilter
            {
                UserId = userId,
                Ip = ip,
                FilterAction = IpFilterAction.Drop,
                Comment = "Created automatically",
                CreatedAt = DateTime.UtcNow
            })
            .ToList();

        if (dropIpFilters.Count == 0)
        {
            return;
        }

        await _dbContext.IpFilters.AddRangeAsync(dropIpFilters, context.CancellationToken);

        await _dbContext.SaveChangesAsync(context.CancellationToken);

        await _mediator.Publish(new CacheInvalidated
        {
            Key = GetUserWithDevicesAndIpFiltersByIdQuery.GetCacheKey(userId)
        }, context.CancellationToken);
    }
}