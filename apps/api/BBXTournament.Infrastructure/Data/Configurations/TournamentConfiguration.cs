using BBXTournament.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BBXTournament.Infrastructure.Data.Configurations;

public class TournamentConfiguration : IEntityTypeConfiguration<Tournament>
{
    public void Configure(EntityTypeBuilder<Tournament> builder)
    {
        builder.ToTable("Tournaments");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.PublicCode)
            .IsRequired()
            .HasMaxLength(20);

        builder.HasIndex(t => t.PublicCode)
            .IsUnique();

        builder.Property(t => t.CommunityId)
            .IsRequired();

        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(t => t.Description)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(t => t.Status)
            .HasConversion<string>()
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(t => t.MaxParticipants)
            .IsRequired();

        builder.Property(t => t.Region)
            .HasMaxLength(100);

        builder.Property(t => t.Province)
            .HasMaxLength(100);

        builder.Property(t => t.City)
            .HasMaxLength(100);

        builder.Property(t => t.CreatedById)
            .IsRequired();

        builder.Property(t => t.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(t => t.DeletedAt);

        builder.Property(t => t.CreatedAt)
            .IsRequired();

        builder.Property(t => t.UpdatedAt)
            .IsRequired();

        // Global query filter to exclude soft-deleted tournaments
        builder.HasQueryFilter(t => !t.IsDeleted);

        // Relationships
        builder.HasOne(t => t.Community)
            .WithMany()
            .HasForeignKey(t => t.CommunityId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(t => t.CreatedBy)
            .WithMany()
            .HasForeignKey(t => t.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(t => t.Stages)
            .WithOne(s => s.Tournament)
            .HasForeignKey(s => s.TournamentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(t => t.Participants)
            .WithOne(p => p.Tournament)
            .HasForeignKey(p => p.TournamentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

// Made with Bob