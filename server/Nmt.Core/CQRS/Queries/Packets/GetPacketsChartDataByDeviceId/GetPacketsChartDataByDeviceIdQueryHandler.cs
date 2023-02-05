using System.Diagnostics;
using MediatR;
using MongoDB.Driver;
using Nmt.Domain.Enums;
using Nmt.Infrastructure.Data.Mongo;

namespace Nmt.Core.CQRS.Queries.Packets.GetPacketsChartDataByDeviceId;

public class GetPacketsChartDataByDeviceIdQueryHandler : IRequestHandler<GetPacketsChartDataByDeviceIdQuery, PacketsChartDataDto>
{
    private readonly MongoDbContext _dbContext;

    public GetPacketsChartDataByDeviceIdQueryHandler(MongoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PacketsChartDataDto> Handle(GetPacketsChartDataByDeviceIdQuery request, CancellationToken cancellationToken)
    {
        var dateTo = DateTime.UtcNow;
        var dateFrom = GetDateFrom(dateTo, request.DateRangeMode);

        var packetStatuses = Enum.GetNames(typeof(PacketStatus));
        var chartData = new PacketsChartDataDto();

        foreach (var status in packetStatuses)
        {
            chartData.Series[status] = new List<int>();
        }

        var packetsAggregate = await GetPacketsAggregate(request.DeviceId, dateFrom, dateTo, request.DateRangeMode, cancellationToken);

        var dateIteratorFrom = GetDateIterator(dateFrom, request.DateRangeMode);
        var dateIteratorTo = GetDateIterator(dateTo, request.DateRangeMode);

        while (dateIteratorFrom <= dateIteratorTo)
        {
            chartData.Categories.Add($"{dateIteratorFrom:O}Z");

            foreach (var status in packetStatuses)
            {
                chartData.Series[status].Add(packetsAggregate.TryGetValue($"{dateIteratorFrom}-{status}", out var count) ? count : 0);
            }

            dateIteratorFrom = request.DateRangeMode == DateRangeMode.Day
                ? dateIteratorFrom.AddHours(1)
                : dateIteratorFrom.AddDays(1);
        }

        return chartData;
    }

    private DateTime GetDateFrom(DateTime dateTo, DateRangeMode dateRangeMode)
    {
        return dateRangeMode switch
        {
            DateRangeMode.Day => dateTo.AddDays(-1).AddHours(1),
            DateRangeMode.Week => dateTo.AddDays(-6),
            _ => throw new UnreachableException()
        };
    }

    private DateTime GetDateIterator(DateTime dateTime, DateRangeMode dateRangeMode)
    {
        return dateRangeMode == DateRangeMode.Day
            ? new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, 0, 0)
            : new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0);
    }

    private async Task<Dictionary<string, int>> GetPacketsAggregate(Guid deviceId, DateTime dateFrom, DateTime dateTo, 
        DateRangeMode dateRangeMode, CancellationToken cancellationToken)
    {
        var packetsAggregate = await _dbContext.Packets
            .Aggregate()
            .Match(p => p.DeviceId == deviceId && p.CreatedAt >= dateFrom && p.CreatedAt <= dateTo)
            .Group(p => new
            {
                DateTime = dateRangeMode == DateRangeMode.Day 
                    ? new DateTime(p.CreatedAt.Year, p.CreatedAt.Month, p.CreatedAt.Day, p.CreatedAt.Hour, 0, 0)
                    : new DateTime(p.CreatedAt.Year, p.CreatedAt.Month, p.CreatedAt.Day, 0, 0, 0),
                Status = p.Status
            }, g => new
            {
                Id = g.Key,
                Count = g.Count()
            })
            .ToListAsync(cancellationToken);

        return packetsAggregate.ToDictionary(k => $"{k.Id.DateTime}-{k.Id.Status}", v => v.Count);
    }
}