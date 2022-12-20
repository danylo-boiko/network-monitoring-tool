using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Nmt.Domain.Models;
using Nmt.Infrastructure.Data.Mongo;
using Nmt.Infrastructure.Data.Postgres;

namespace Nmt.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var postgresConnectionString = configuration.GetConnectionString("Postgres");
        services.AddNpgsql<PostgresDbContext>(postgresConnectionString);

        var mongoDbSettings = configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();
        services.AddSingleton(mongoDbSettings!);

        services.AddTransient<IMongoDatabase>(provider =>
        {
            var settings = provider.GetRequiredService<MongoDbSettings>();
            var mongoConnectionString = configuration.GetConnectionString("Mongo");
            
            var client = new MongoClient(mongoConnectionString);
            return client.GetDatabase(settings.DatabaseName);
        });
        
        return services;
    }

    public static IServiceCollection ConfigureIdentity(this IServiceCollection services)
    {
        services.AddIdentity<User, Role>(o => 
        {
            o.Password.RequireDigit = true;
            o.Password.RequireLowercase = true;
            o.Password.RequireUppercase = true;
            o.Password.RequiredLength = 8;

            o.User.RequireUniqueEmail = true;
        })
        .AddEntityFrameworkStores<PostgresDbContext>()
        .AddDefaultTokenProviders();

        return services;
    }
}