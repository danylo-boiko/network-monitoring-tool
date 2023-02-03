using MediatR;
using MongoDB.Driver;
using Nmt.Domain.Models;
using Nmt.Infrastructure.Data.Mongo;

namespace Nmt.Core.CQRS.Queries.Packets.GetPacketsByDeviceId;

public class GetPacketsByDeviceIdQueryHandler : IRequestHandler<GetPacketsByDeviceIdQuery, IList<PacketDto>>
{
    private readonly MongoDbContext _dbContext;

    public GetPacketsByDeviceIdQueryHandler(MongoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IList<PacketDto>> Handle(GetPacketsByDeviceIdQuery request, CancellationToken cancellationToken)
    {
        var filterBuilder = Builders<Packet>.Filter;
        var filter = filterBuilder.Eq(p => p.DeviceId, request.DeviceId);

        if (request.DateFrom.HasValue)
        {
            filter &= filterBuilder.Gte(p => p.CreatedAt, request.DateFrom.Value);
        }

        if (request.DateTo.HasValue)
        {
            filter &= filterBuilder.Lte(p => p.CreatedAt, request.DateTo.Value);
        }

        return await _dbContext.Packets
            .Find(filter)
            .Project(p => new PacketDto
            {
                Id = p.Id,
                Ip = p.Ip,
                Size = p.Size,
                Protocol = p.Protocol,
                Status = p.Status,
                CreatedAt = p.CreatedAt
            })
            .ToListAsync(cancellationToken);
    }
}