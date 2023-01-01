using Microsoft.EntityFrameworkCore;
using Nmt.Domain.Consts;
using Nmt.Domain.Enums;
using Nmt.Domain.Models;

namespace Nmt.Infrastructure.Data.Postgres.Extensions;

public static class DbSeed
{
    private static int _nextRoleClaimId = 1;

    public static ModelBuilder SeedRolesAndClaims(this ModelBuilder builder)
    {
        var userRole = new Role
        {
            Id = Guid.NewGuid(),
            Name = nameof(UserRoles.User),
            NormalizedName = nameof(UserRoles.User).ToUpper()
        };

        var userRolePermissionClaims = GetRolePermissionClaims(userRole.Id, new HashSet<Permissions>
        {
            Permissions.UsersDelete,
            Permissions.PacketsUpdate,
            Permissions.PacketsDelete
        });

        var adminRole = new Role
        {
            Id = Guid.NewGuid(),
            Name = nameof(UserRoles.Admin),
            NormalizedName = nameof(UserRoles.Admin).ToUpper()
        };

        var adminRolePermissionClaims = GetRolePermissionClaims(adminRole.Id);

        builder
            .Entity<Role>()
            .HasData(new List<Role> { userRole, adminRole });

        builder
            .Entity<RoleClaim>()
            .HasData(userRolePermissionClaims.Concat(adminRolePermissionClaims));

        return builder;
    }

    private static IList<RoleClaim> GetRolePermissionClaims(Guid roleId, ISet<Permissions>? excludeFilter = null)
    {
        var permissions = new List<RoleClaim>();

        foreach (var permission in (Permissions[])Enum.GetValues(typeof(Permissions)))
        {
            if (excludeFilter == null || !excludeFilter.Contains(permission))
            {
                permissions.Add(new RoleClaim
                {
                    Id = _nextRoleClaimId,
                    RoleId = roleId,
                    ClaimType = nameof(Permissions),
                    ClaimValue = permission.GetHashCode().ToString()
                });

                _nextRoleClaimId += 1;
            }
        }

        return permissions;
    }
}