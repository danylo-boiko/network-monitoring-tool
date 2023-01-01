using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nmt.Domain.Models;

namespace Nmt.Infrastructure.Data.Postgres.EntityConfigurations;

public class BlockedIpEntityTypeConfiguration : IEntityTypeConfiguration<BlockedIp>
{
    public void Configure(EntityTypeBuilder<BlockedIp> builder)
    {
        builder.HasKey(bi => bi.Id);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(bi => bi.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(bi => bi.Ip).IsRequired();

        builder.Property(bi => bi.Reason).IsRequired(false);
    }
}