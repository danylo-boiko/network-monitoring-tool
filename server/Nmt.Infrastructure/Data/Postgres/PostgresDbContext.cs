using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Nmt.Domain.Models;
using Nmt.Infrastructure.Data.Postgres.Extensions;

namespace Nmt.Infrastructure.Data.Postgres;

public class PostgresDbContext : IdentityDbContext<User, Role, Guid>
{
    public virtual DbSet<Device> Devices { get; set; }
    public virtual DbSet<BlockedIp> BlockedIps { get; set; }

    public PostgresDbContext(DbContextOptions<PostgresDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder
            .ApplyEntityTypesConfigurations()
            .SeedRoles();
    }
}