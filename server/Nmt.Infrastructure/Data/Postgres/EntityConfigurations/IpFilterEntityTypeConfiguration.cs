using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nmt.Domain.Models;

namespace Nmt.Infrastructure.Data.Postgres.EntityConfigurations;

public class IpFilterEntityTypeConfiguration : IEntityTypeConfiguration<IpFilter>
{
    public void Configure(EntityTypeBuilder<IpFilter> builder)
    {
        builder.HasKey(i => i.Id);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(i => i.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(i => i.Ip).IsRequired();

        builder.Property(i => i.FilterAction).IsRequired();

        builder.Property(i => i.Comment).IsRequired(false);
    }
}