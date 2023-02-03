using MediatR;
using Microsoft.EntityFrameworkCore;
using Nmt.Core.CQRS.Queries.Users.GetUserWithDevicesAndIpFiltersById;
using Nmt.Domain.Consts;
using Nmt.Domain.Events;
using Nmt.Domain.Exceptions;
using Nmt.Infrastructure.Data.Postgres;

namespace Nmt.Core.CQRS.Commands.IpFilters.DeleteIpFilter;

public class DeleteIpFilterCommandHandler : IRequestHandler<DeleteIpFilterCommand, bool>
{
    private readonly PostgresDbContext _dbContext;
    private readonly IMediator _mediator;

    public DeleteIpFilterCommandHandler(PostgresDbContext dbContext, IMediator mediator)
    {
        _dbContext = dbContext;
        _mediator = mediator;
    }

    public async Task<bool> Handle(DeleteIpFilterCommand request, CancellationToken cancellationToken)
    {
        var ipFilter = await _dbContext.IpFilters.FirstOrDefaultAsync(i => i.Id == request.IpFilterId, cancellationToken);
        if (ipFilter == null)
        {
            throw new DomainException($"IP filter with id '{request.IpFilterId}' not found")
            {
                Code = ExceptionCodes.NotFound,
                Property = nameof(request.IpFilterId)
            };
        }

        _dbContext.IpFilters.Remove(ipFilter);

        await _dbContext.SaveChangesAsync(cancellationToken);

        await _mediator.Publish(new CacheInvalidated
        {
            Key = GetUserWithDevicesAndIpFiltersByIdQuery.GetCacheKey(ipFilter.UserId)
        }, cancellationToken);

        return true;
    }
}