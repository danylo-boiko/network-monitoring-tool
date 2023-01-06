using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Nmt.Domain.Configs;
using Nmt.Domain.Models;
using Nmt.Infrastructure.Cache.MemoryCache;
using Nmt.Infrastructure.Cache.MemoryCache.Interfaces;
using Nmt.Infrastructure.Cache.Redis;
using Nmt.Infrastructure.Cache.Redis.Interfaces;
using Nmt.Infrastructure.Data.Mongo;
using Nmt.Infrastructure.Data.Postgres;

namespace Nmt.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // PostgreSQL
        var postgresConnectionString = configuration.GetConnectionString("Postgres");
        services.AddNpgsql<PostgresDbContext>(postgresConnectionString);

        services.ConfigurePostgres();

        // MongoDB
        var mongoConnectionString = configuration.GetConnectionString("Mongo");
        var mongoDbConfig = configuration.GetSection(nameof(MongoDbConfig)).Get<MongoDbConfig>();

        services.AddTransient<MongoDbContext>(_ =>
        {
            var client = new MongoClient(mongoConnectionString);
            var database = client.GetDatabase(mongoDbConfig!.DatabaseName);

            return new MongoDbContext(database, mongoDbConfig);
        });

        services.ConfigureMongo();

        // Redis cache
        var redisConnectionString = configuration.GetConnectionString("Redis");
        services.AddDistributedRedisCache(opts =>
        {
            opts.Configuration = redisConnectionString;
        });

        services.AddTransient<IDistributedRedisCache, DistributedRedisCache>();

        // Memory cache
        services.AddMemoryCache();

        services.AddTransient<IMemoryCache, MemoryCache>();

        return services;
    }

    private static IServiceCollection ConfigurePostgres(this IServiceCollection services)
    {
        services.AddIdentity<User, Role>(o => 
        {
            o.Password.RequireDigit = true;
            o.Password.RequireLowercase = true;
            o.Password.RequireUppercase = true;
            o.Password.RequireNonAlphanumeric = false;
            o.Password.RequiredLength = 8;

            o.User.RequireUniqueEmail = true;
        })
        .AddEntityFrameworkStores<PostgresDbContext>()
        .AddDefaultTokenProviders();

        return services;
    }

    private static IServiceCollection ConfigureMongo(this IServiceCollection services)
    {
        var mongoDbContext = services.BuildServiceProvider().GetService<MongoDbContext>()!;

        var packetsIndex = Builders<Packet>.IndexKeys
            .Ascending(p => p.DeviceId)
            .Ascending(p => p.CreatedAt);

        mongoDbContext.Packets.Indexes.CreateOne(new CreateIndexModel<Packet>(packetsIndex));

        return services;
    }
}