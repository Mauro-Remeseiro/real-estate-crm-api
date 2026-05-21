using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstateCrmApi.Domain.Entities;

namespace RealEstateCrmApi.Infrastructure.Persistence.Configurations;

public class VisitConfiguration : IEntityTypeConfiguration<Visit>
{
    public void Configure(EntityTypeBuilder<Visit> builder)
    {
        builder.ConfigureBaseEntity();

        builder.ToTable("Visits");

        builder.Property(v => v.ScheduledAt)
            .IsRequired();

        builder.Property(v => v.Status)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(v => v.Notes)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(v => v.PropertyId)
            .IsRequired();

        builder.Property(v => v.ClientId)
            .IsRequired();

        builder.Property(v => v.AgentId)
            .IsRequired();

        builder.HasOne<Property>()
            .WithMany()
            .HasForeignKey(v => v.PropertyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Client>()
            .WithMany()
            .HasForeignKey(v => v.ClientId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(v => v.AgentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(v => v.PropertyId);
        builder.HasIndex(v => v.ClientId);
        builder.HasIndex(v => v.AgentId);
        builder.HasIndex(v => v.ScheduledAt);
    }
}
