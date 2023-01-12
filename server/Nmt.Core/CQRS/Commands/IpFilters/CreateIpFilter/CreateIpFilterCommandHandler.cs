using LS.Helpers.Hosting.API;
using MediatR;
using Nmt.Core.CQRS.Queries.Users.GetUserWithDevicesAndIpFiltersById;
using Nmt.Domain.Events;
using Nmt.Domain.Models;
using Nmt.Infrastructure.Data.Postgres;

namespace Nmt.Core.CQRS.Commands.IpFilters.CreateIpFilter;

public class CreateIpFilterCommandHandler : IRequestHandler<CreateIpFilterCommand, ExecutionResult<Guid>>
{
    private readonly PostgresDbContext _dbContext;
    private readonly IMediator _mediator;

    public CreateIpFilterCommandHandler(PostgresDbContext dbContext, IMediator mediator)
    {
        _dbContext = dbContext;
        _mediator = mediator;
    }

    public async Task<ExecutionResult<Guid>> Handle(CreateIpFilterCommand request, CancellationToken cancellationToken)
    {
        var ipFilter = new IpFilter
        {
            UserId = request.UserId,
            Ip = (uint)request.Ip,
            FilterAction = request.FilterAction,
            Comment = request.Comment,
            CreatedAt = DateTime.UtcNow
        };

        await _dbContext.IpFilters.AddAsync(ipFilter, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);

        await _mediator.Publish(new CacheInvalidated
        {
            Key = GetUserWithDevicesAndIpFiltersByIdQuery.GetCacheKey(ipFilter.UserId)
        }, cancellationToken);

        return new ExecutionResult<Guid>(ipFilter.Id);
    }
}