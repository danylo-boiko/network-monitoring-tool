using MassTransit;
using MediatR;
using Nmt.Domain.BusEvents;
using Nmt.Domain.Enums;
using Nmt.Infrastructure.Cache.MemoryCache.Interfaces;
using Nmt.Infrastructure.Data.Mongo;

namespace Nmt.Core.CQRS.Commands.Packets.CreatePackets;

public class CreatePacketsCommandHandler : IRequestHandler<CreatePacketsCommand>
{
    private readonly MongoDbContext _dbContext;
    private readonly IMemoryCache _memoryCache;
    private readonly IPublishEndpoint _publishEndpoint;

    private const int ComparisonMultiplier = 7;

    public CreatePacketsCommandHandler(
        MongoDbContext dbContext, 
        IMemoryCache memoryCache, 
        IPublishEndpoint publishEndpoint)
    {
        _dbContext = dbContext;
        _memoryCache = memoryCache;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<Unit> Handle(CreatePacketsCommand request, CancellationToken cancellationToken)
    {
        await _dbContext.Packets.InsertManyAsync(request.Packets, cancellationToken: cancellationToken);

        var passedPackets = request.Packets
            .Where(p => p.Status == PacketStatus.Passed)
            .ToList();

        var countOfUniqueIps = passedPackets
            .Select(p => p.Ip)
            .Distinct()
            .Count();

        var packetsAggregate = new PacketsAggregate
        {
            CountOfUniqueIps = countOfUniqueIps,
            AverageCountOfPacketsPerIp = (float)passedPackets.Count / (float)countOfUniqueIps
        };

        var packetsAggregateCacheKey = PacketsAggregate.GetCacheKey(request.DeviceId);
        var cachedPacketsAggregate = _memoryCache.Get<PacketsAggregate>(packetsAggregateCacheKey);

        if (cachedPacketsAggregate != null && packetsAggregate.HasAnomaly(cachedPacketsAggregate, ComparisonMultiplier))
        {
            await _publishEndpoint.Publish(new DetectDdosAttacksEvent
            {
                DeviceId = request.DeviceId,
                DateFrom = passedPackets.Min(p => p.CreatedAt),
                DateTo = passedPackets.Max(p => p.CreatedAt)
            }, cancellationToken);
        }

        _memoryCache.Set(packetsAggregateCacheKey, packetsAggregate, TimeSpan.FromMinutes(1));

        return Unit.Value;
    }
}