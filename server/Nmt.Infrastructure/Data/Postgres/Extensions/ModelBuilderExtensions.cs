using Microsoft.EntityFrameworkCore;
using Nmt.Infrastructure.Data.Postgres.EntityConfigurations;

namespace Nmt.Infrastructure.Data.Postgres.Extensions;

public static class ModelBuilderExtensions
{
    public static ModelBuilder ApplyEntityTypesConfigurations(this ModelBuilder builder)
    {
        builder.ApplyConfiguration(new UserEntityTypeConfiguration());
        builder.ApplyConfiguration(new DeviceEntityTypeConfiguration());
        builder.ApplyConfiguration(new IpFilterEntityTypeConfiguration());

        return builder;
    }
}