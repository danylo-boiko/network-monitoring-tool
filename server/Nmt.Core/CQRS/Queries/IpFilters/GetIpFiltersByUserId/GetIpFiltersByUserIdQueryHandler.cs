using MediatR;
using Microsoft.EntityFrameworkCore;
using Nmt.Infrastructure.Data.Postgres;

namespace Nmt.Core.CQRS.Queries.IpFilters.GetIpFiltersByUserId;

public class GetIpFiltersByUserIdQueryHandler : IRequestHandler<GetIpFiltersByUserIdQuery, IList<IpFilterDto>>
{
    private readonly PostgresDbContext _dbContext;

    public GetIpFiltersByUserIdQueryHandler(PostgresDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IList<IpFilterDto>> Handle(GetIpFiltersByUserIdQuery request, CancellationToken cancellationToken)
    {
        return await _dbContext.IpFilters
            .Where(i => i.UserId == request.UserId)
            .Select(i => new IpFilterDto
            {
                Id = i.Id,
                Ip = i.Ip,
                FilterAction = i.FilterAction,
                CreatedAt = i.CreatedAt
            }).ToListAsync(cancellationToken);
    }
}