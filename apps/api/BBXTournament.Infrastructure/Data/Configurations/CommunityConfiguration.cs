using BBXTournament.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BBXTournament.Infrastructure.Data.Configurations;

public class CommunityConfiguration : IEntityTypeConfiguration<Community>
{
    public void Configure(EntityTypeBuilder<Community> builder)
    {
        builder.ToTable("Communities");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.Slug)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasIndex(c => c.Slug)
            .IsUnique();

        builder.Property(c => c.Description)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(c => c.LogoUrl)
            .HasMaxLength(500);

        builder.Property(c => c.BannerUrl)
            .HasMaxLength(500);

        builder.Property(c => c.Region)
            .HasMaxLength(100);

        builder.Property(c => c.Province)
            .HasMaxLength(100);

        builder.Property(c => c.City)
            .HasMaxLength(100);

        builder.Property(c => c.OwnerId)
            .IsRequired();

        builder.Property(c => c.IsVerified)
            .IsRequired();

        builder.Property(c => c.IsActive)
            .IsRequired();

        builder.Property(c => c.CreatedAt)
            .IsRequired();

        builder.Property(c => c.UpdatedAt)
            .IsRequired();

        // Relationships
        builder.HasOne(c => c.Owner)
            .WithMany()
            .HasForeignKey(c => c.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(c => c.Admins)
            .WithOne(a => a.Community)
            .HasForeignKey(a => a.CommunityId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(c => c.Members)
            .WithOne(m => m.Community)
            .HasForeignKey(m => m.CommunityId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

// Made with Bob