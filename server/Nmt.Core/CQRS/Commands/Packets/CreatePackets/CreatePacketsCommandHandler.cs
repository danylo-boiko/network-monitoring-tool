using MediatR;
using Nmt.Infrastructure.Data.Mongo;

namespace Nmt.Core.CQRS.Commands.Packets.CreatePackets;

public class CreatePacketsCommandHandler : IRequestHandler<CreatePacketsCommand>
{
    private readonly MongoDbContext _mongoDbContext;

    public CreatePacketsCommandHandler(MongoDbContext mongoDbContext)
    {
        _mongoDbContext = mongoDbContext;
    }

    public async Task<Unit> Handle(CreatePacketsCommand request, CancellationToken cancellationToken)
    {
        await _mongoDbContext.Packets.InsertManyAsync(request.Packets, cancellationToken: cancellationToken);

        return Unit.Value;
    }
}