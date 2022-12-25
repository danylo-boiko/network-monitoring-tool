using MongoDB.Driver;
using Nmt.Domain.Models;

namespace Nmt.Infrastructure.Data.Mongo.Extensions;

public static class IndexBuilderExtensions
{
    public static IMongoDatabase ApplyIndexesConfiguration(this IMongoDatabase database, MongoDbSettings settings)
    {
        var packetsBuilder = Builders<Packet>.IndexKeys;
        var packetsCollection = database.GetCollection<Packet>(settings.PacketsCollectionName);

        var indexModel = new CreateIndexModel<Packet>(packetsBuilder.Ascending(p => p.DeviceId));
        packetsCollection.Indexes.CreateOne(indexModel);

        return database;
    }
}