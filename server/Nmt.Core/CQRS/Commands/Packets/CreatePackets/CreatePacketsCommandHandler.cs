using MediatR;
using Microsoft.Extensions.Logging;
using Nmt.Infrastructure.Cache.MemoryCache.Interfaces;
using Nmt.Infrastructure.Data.Mongo;

namespace Nmt.Core.CQRS.Commands.Packets.CreatePackets;

public class CreatePacketsCommandHandler : IRequestHandler<CreatePacketsCommand>
{
    private readonly MongoDbContext _dbContext;
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger<CreatePacketsCommandHandler> _logger;

    private const int ComparisonMultiplier = 10;

    public CreatePacketsCommandHandler(
        MongoDbContext dbContext, 
        IMemoryCache memoryCache, 
        ILogger<CreatePacketsCommandHandler> logger)
    {
        _dbContext = dbContext;
        _memoryCache = memoryCache;
        _logger = logger;
    }

    public async Task<Unit> Handle(CreatePacketsCommand request, CancellationToken cancellationToken)
    {
        await _dbContext.Packets.InsertManyAsync(request.Packets, cancellationToken: cancellationToken);

        var packetsAggregateCacheKey = PacketsAggregate.GetCacheKey(request.DeviceId);
        var countOfUniqueIps = request.Packets.Select(p => p.Ip).Distinct().Count();

        var packetsAggregate = new PacketsAggregate
        {
            CountOfUniqueIps = countOfUniqueIps,
            AverageCountOfPacketsPerIp = (float)request.Packets.Count / (float)countOfUniqueIps
        };

        var cachedPacketsAggregate = _memoryCache.Get<PacketsAggregate>(packetsAggregateCacheKey);
        if (cachedPacketsAggregate != null && packetsAggregate.HasAnomaly(cachedPacketsAggregate, ComparisonMultiplier))
        {
            _logger.LogInformation($"Detected anomaly for device with id {request.DeviceId}");
        }

        _memoryCache.Set(packetsAggregateCacheKey, packetsAggregate, TimeSpan.FromMinutes(1));

        return Unit.Value;
    }
}