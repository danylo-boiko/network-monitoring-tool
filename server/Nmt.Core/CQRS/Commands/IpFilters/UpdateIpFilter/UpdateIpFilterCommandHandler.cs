using LS.Helpers.Hosting.API;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nmt.Core.CQRS.Queries.Users.GetUserWithDevicesAndIpFiltersById;
using Nmt.Domain.Events;
using Nmt.Infrastructure.Data.Postgres;

namespace Nmt.Core.CQRS.Commands.IpFilters.UpdateIpFilter;

public class UpdateIpFilterCommandHandler : IRequestHandler<UpdateIpFilterCommand, ExecutionResult<bool>>
{
    private readonly PostgresDbContext _dbContext;
    private readonly IMediator _mediator;

    public UpdateIpFilterCommandHandler(PostgresDbContext dbContext, IMediator mediator)
    {
        _dbContext = dbContext;
        _mediator = mediator;
    }
    
    public async Task<ExecutionResult<bool>> Handle(UpdateIpFilterCommand request, CancellationToken cancellationToken)
    {
        var ipFilter = await _dbContext.IpFilters.FirstOrDefaultAsync(i => i.Id == request.IpFilterId, cancellationToken);
        if (ipFilter == null)
        {
            return new ExecutionResult<bool>(new ErrorInfo(nameof(request.IpFilterId), $"IP filter with id '{request.IpFilterId}' not found"));
        }

        ipFilter.FilterAction = request.FilterAction;
        ipFilter.Comment = request.Comment;

        await _dbContext.SaveChangesAsync(cancellationToken);

        await _mediator.Publish(new CacheInvalidated
        {
            Key = GetUserWithDevicesAndIpFiltersByIdQuery.GetCacheKey(ipFilter.UserId)
        }, cancellationToken);

        return new ExecutionResult<bool>(true);
    }
}