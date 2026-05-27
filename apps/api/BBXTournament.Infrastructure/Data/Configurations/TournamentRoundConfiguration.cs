using BBXTournament.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BBXTournament.Infrastructure.Data.Configurations;

public class TournamentRoundConfiguration : IEntityTypeConfiguration<TournamentRound>
{
    public void Configure(EntityTypeBuilder<TournamentRound> builder)
    {
        builder.ToTable("TournamentRounds");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.TournamentStageId)
            .IsRequired();

        builder.Property(r => r.RoundNumber)
            .IsRequired();

        builder.Property(r => r.Status)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(r => r.StartedAt)
            .IsRequired(false);

        builder.Property(r => r.CompletedAt)
            .IsRequired(false);

        builder.Property(r => r.CreatedAt)
            .IsRequired();

        // Relationships
        builder.HasOne(r => r.TournamentStage)
            .WithMany(s => s.Rounds)
            .HasForeignKey(r => r.TournamentStageId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(r => r.Matches)
            .WithOne(m => m.TournamentRound)
            .HasForeignKey(m => m.TournamentRoundId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex(r => r.TournamentStageId);
        builder.HasIndex(r => new { r.TournamentStageId, r.RoundNumber })
            .IsUnique();
    }
}

// Made with Bob