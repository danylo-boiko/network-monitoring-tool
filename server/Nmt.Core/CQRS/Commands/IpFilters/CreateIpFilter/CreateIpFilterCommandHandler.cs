using LS.Helpers.Hosting.API;
using MediatR;
using Nmt.Domain.Models;
using Nmt.Infrastructure.Data.Postgres;

namespace Nmt.Core.CQRS.Commands.IpFilters.CreateIpFilter;

public class CreateIpFilterCommandHandler : IRequestHandler<CreateIpFilterCommand, ExecutionResult>
{
    private readonly PostgresDbContext _dbContext;

    public CreateIpFilterCommandHandler(PostgresDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ExecutionResult> Handle(CreateIpFilterCommand request, CancellationToken cancellationToken)
    {
        await _dbContext.IpFilters.AddAsync(new IpFilter
        {
            UserId = request.UserId,
            Ip = request.Ip,
            FilterAction = request.FilterAction,
            Comment = request.Comment,
            CreatedAt = DateTime.UtcNow
        }, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new ExecutionResult();
    }
}