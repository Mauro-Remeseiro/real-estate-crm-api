using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstateCrmApi.Domain.Entities;

namespace RealEstateCrmApi.Infrastructure.Persistence.Configurations;

public class PropertyConfiguration : IEntityTypeConfiguration<Property>
{
    public void Configure(EntityTypeBuilder<Property> builder)
    {
        builder.ConfigureBaseEntity();

        builder.ToTable("Properties");

        builder.Property(p => p.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.Description)
            .IsRequired()
            .HasMaxLength(4000);

        builder.Property(p => p.Address)
            .IsRequired()
            .HasMaxLength(300);

        builder.Property(p => p.City)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.Price)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(p => p.Type)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(p => p.Status)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(p => p.Bedrooms)
            .IsRequired();

        builder.Property(p => p.Bathrooms)
            .IsRequired();

        builder.Property(p => p.SquareMeters)
            .IsRequired();

        builder.Property(p => p.AssignedUserId)
            .IsRequired();

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(p => p.AssignedUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(p => p.Status);
        builder.HasIndex(p => p.City);
    }
}
