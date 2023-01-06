using LS.Helpers.Hosting.API;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nmt.Core.CQRS.Queries.Users.GetUserWithDevicesAndIpFiltersById;
using Nmt.Domain.Consts;
using Nmt.Domain.Events;
using Nmt.Infrastructure.Data.Postgres;

namespace Nmt.Core.CQRS.Commands.IpFilters.DeleteIpFilter;

public class DeleteIpFilterCommandHandler : IRequestHandler<DeleteIpFilterCommand, ExecutionResult<bool>>
{
    private readonly PostgresDbContext _dbContext;
    private readonly IMediator _mediator;

    public DeleteIpFilterCommandHandler(PostgresDbContext dbContext, IMediator mediator)
    {
        _dbContext = dbContext;
        _mediator = mediator;
    }

    public async Task<ExecutionResult<bool>> Handle(DeleteIpFilterCommand request, CancellationToken cancellationToken)
    {
        var ipFilter = await _dbContext.IpFilters.FirstOrDefaultAsync(i => i.Id == request.IpFilterId, cancellationToken);
        if (ipFilter == null)
        {
            return new ExecutionResult<bool>(new ErrorInfo(StatusCodes.NotFound, $"IP filter with id '{request.IpFilterId}' not found"));
        }

        _dbContext.IpFilters.Remove(ipFilter);

        await _dbContext.SaveChangesAsync(cancellationToken);
        await _mediator.Publish(new CacheInvalidated
        {
            Key = GetUserWithDevicesAndIpFiltersByIdQuery.GetCacheKey(ipFilter.UserId)
        }, cancellationToken);
        
        return new ExecutionResult<bool>(true);
    }
}