using MediatR;
using Nmt.Infrastructure.Data.Mongo;

namespace Nmt.Core.CQRS.Commands.Packets.CreatePackets;

public class CreatePacketsCommandHandler : IRequestHandler<CreatePacketsCommand>
{
    private readonly MongoDbContext _dbContext;

    public CreatePacketsCommandHandler(MongoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Unit> Handle(CreatePacketsCommand request, CancellationToken cancellationToken)
    {
        await _dbContext.Packets.InsertManyAsync(request.Packets, cancellationToken: cancellationToken);

        return Unit.Value;
    }
}