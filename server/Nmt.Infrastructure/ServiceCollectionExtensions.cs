using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nmt.Infrastructure.Data.Postgres;

namespace Nmt.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var postgresConnectionString = configuration.GetConnectionString("Postgres");
        services.AddNpgsql<PostgresDbContext>(postgresConnectionString);

        return services;
    }
}