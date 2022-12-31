using Microsoft.EntityFrameworkCore;
using Nmt.Domain.Consts;
using Nmt.Domain.Models;

namespace Nmt.Infrastructure.Data.Postgres.Extensions;

public static class DbSeed
{
    public static ModelBuilder SeedRoles(this ModelBuilder builder)
    {
        var roles = new List<Role>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = nameof(UserRoles.User),
                NormalizedName = nameof(UserRoles.User).ToUpper()
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = nameof(UserRoles.Moderator),
                NormalizedName = nameof(UserRoles.Moderator).ToUpper()
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = nameof(UserRoles.Admin),
                NormalizedName = nameof(UserRoles.Admin).ToUpper()
            }
        };

        builder
            .Entity<Role>()
            .HasData(roles);

        return builder;
    }
}