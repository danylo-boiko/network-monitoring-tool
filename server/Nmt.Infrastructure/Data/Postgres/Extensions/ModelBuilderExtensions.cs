using Microsoft.EntityFrameworkCore;

namespace Nmt.Infrastructure.Data.Postgres.Extensions;

public static class ModelBuilderExtensions
{
    public static ModelBuilder ApplyEntityTypesConfigurations(this ModelBuilder builder)
    {
        return builder;
    }
}