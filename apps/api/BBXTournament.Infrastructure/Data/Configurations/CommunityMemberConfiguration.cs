using BBXTournament.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BBXTournament.Infrastructure.Data.Configurations;

public class CommunityMemberConfiguration : IEntityTypeConfiguration<CommunityMember>
{
    public void Configure(EntityTypeBuilder<CommunityMember> builder)
    {
        builder.ToTable("CommunityMembers");

        builder.HasKey(cm => cm.Id);

        builder.Property(cm => cm.CommunityId)
            .IsRequired();

        builder.Property(cm => cm.UserId)
            .IsRequired();

        builder.Property(cm => cm.JoinedAt)
            .IsRequired();

        // Unique constraint: one user can only be a member once per community
        builder.HasIndex(cm => new { cm.CommunityId, cm.UserId })
            .IsUnique();

        // Relationships
        builder.HasOne(cm => cm.Community)
            .WithMany(c => c.Members)
            .HasForeignKey(cm => cm.CommunityId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(cm => cm.User)
            .WithMany()
            .HasForeignKey(cm => cm.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

// Made with Bob