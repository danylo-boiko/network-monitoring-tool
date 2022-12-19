using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Nmt.Core.Models;
using Nmt.Infrastructure.Data.Postgres.EntityConfigurations;

namespace Nmt.Infrastructure.Data.Postgres.Extensions;

public static class ModelBuilderExtensions
{
    public static ModelBuilder ApplyEntityTypesConfigurations(this ModelBuilder builder)
    {
        builder.ApplyConfiguration(new UserEntityTypeConfiguration());

        return builder;
    }

    public static ModelBuilder ApplyIdentityConfiguration(this ModelBuilder builder)
    {
        builder.Ignore<IdentityUserClaim<Guid>>();
        builder.Ignore<IdentityUserLogin<Guid>>();
        builder.Ignore<IdentityUserToken<Guid>>();

        return builder;
    }
}