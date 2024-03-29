using MongoDB.Driver;
using Nmt.Domain.Configs;
using Nmt.Domain.Models;

namespace Nmt.Infrastructure.Data.Mongo;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;
    private readonly MongoDbConfig _config;

    public virtual IMongoCollection<Packet> Packets => _database.GetCollection<Packet>(_config.PacketsCollectionName);

    public MongoDbContext(IMongoDatabase database, MongoDbConfig config)
    {
        _database = database;
        _config = config;
    }
}