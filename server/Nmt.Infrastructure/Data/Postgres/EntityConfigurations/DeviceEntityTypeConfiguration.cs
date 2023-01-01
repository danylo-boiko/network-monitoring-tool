using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nmt.Domain.Models;

namespace Nmt.Infrastructure.Data.Postgres.EntityConfigurations;

public class DeviceEntityTypeConfiguration : IEntityTypeConfiguration<Device>
{
    public void Configure(EntityTypeBuilder<Device> builder)
    {
        builder.HasKey(d => d.Id);

        builder.HasIndex(d => new { d.UserId, d.MachineSpecificStamp });

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(d => d.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(d => d.MachineSpecificStamp).IsRequired();
    }
}