using MediatR;
using Microsoft.EntityFrameworkCore;
using Nmt.Core.CQRS.Queries.Users.GetUserWithDevicesAndIpFiltersById;
using Nmt.Domain.Consts;
using Nmt.Domain.Events;
using Nmt.Domain.Exceptions;
using Nmt.Domain.Models;
using Nmt.Infrastructure.Data.Postgres;

namespace Nmt.Core.CQRS.Commands.IpFilters.CreateIpFilter;

public class CreateIpFilterCommandHandler : IRequestHandler<CreateIpFilterCommand, Guid>
{
    private readonly PostgresDbContext _dbContext;
    private readonly IMediator _mediator;

    public CreateIpFilterCommandHandler(PostgresDbContext dbContext, IMediator mediator)
    {
        _dbContext = dbContext;
        _mediator = mediator;
    }

    public async Task<Guid> Handle(CreateIpFilterCommand request, CancellationToken cancellationToken)
    {
        var isIpFilterDuplicated = await _dbContext.IpFilters.AnyAsync(i => i.UserId == request.UserId && i.Ip == (uint)request.Ip, cancellationToken);
        if (isIpFilterDuplicated)
        {
            throw new DomainException("Filter for this IP already exists")
            {
                Code = ExceptionCodes.AlreadyExists,
                Property = nameof(request.Ip)
            };
        }

        var ipFilter = new IpFilter
        {
            UserId = request.UserId!.Value,
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

        return ipFilter.Id;
    }
}