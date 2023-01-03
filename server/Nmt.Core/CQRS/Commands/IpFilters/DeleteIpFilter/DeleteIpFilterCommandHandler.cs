using LS.Helpers.Hosting.API;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nmt.Infrastructure.Data.Postgres;

namespace Nmt.Core.CQRS.Commands.IpFilters.DeleteIpFilter;

public class DeleteIpFilterCommandHandler : IRequestHandler<DeleteIpFilterCommand, ExecutionResult>
{
    private readonly PostgresDbContext _dbContext;

    public DeleteIpFilterCommandHandler(PostgresDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ExecutionResult> Handle(DeleteIpFilterCommand request, CancellationToken cancellationToken)
    {
        var ipFilter = await _dbContext.IpFilters.FirstOrDefaultAsync(i => i.Id == request.IpFilterId, cancellationToken);
        if (ipFilter == null)
        {
            return new ExecutionResult(new ErrorInfo($"IP filter with id '{request.IpFilterId}' not found"));
        }

        _dbContext.IpFilters.Remove(ipFilter);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new ExecutionResult();
    }
}