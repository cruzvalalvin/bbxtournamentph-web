using BBXTournament.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BBXTournament.Infrastructure.Data.Configurations;

public class CommunityAdminConfiguration : IEntityTypeConfiguration<CommunityAdmin>
{
    public void Configure(EntityTypeBuilder<CommunityAdmin> builder)
    {
        builder.ToTable("CommunityAdmins");

        builder.HasKey(ca => ca.Id);

        builder.Property(ca => ca.CommunityId)
            .IsRequired();

        builder.Property(ca => ca.UserId)
            .IsRequired();

        builder.Property(ca => ca.Role)
            .HasConversion<string>()
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(ca => ca.CreatedAt)
            .IsRequired();

        // Unique constraint: one user can only have one admin role per community
        builder.HasIndex(ca => new { ca.CommunityId, ca.UserId })
            .IsUnique();

        // Relationships
        builder.HasOne(ca => ca.Community)
            .WithMany(c => c.Admins)
            .HasForeignKey(ca => ca.CommunityId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ca => ca.User)
            .WithMany()
            .HasForeignKey(ca => ca.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

// Made with Bob