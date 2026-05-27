using BBXTournament.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BBXTournament.Infrastructure.Data.Configurations;

public class StandingConfiguration : IEntityTypeConfiguration<Standing>
{
    public void Configure(EntityTypeBuilder<Standing> builder)
    {
        builder.ToTable("Standings");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.TournamentStageId)
            .IsRequired();

        builder.Property(s => s.ParticipantId)
            .IsRequired();

        builder.Property(s => s.Wins)
            .IsRequired();

        builder.Property(s => s.Losses)
            .IsRequired();

        builder.Property(s => s.Draws)
            .IsRequired();

        builder.Property(s => s.MatchPoints)
            .IsRequired();

        builder.Property(s => s.PointDifference)
            .IsRequired();

        builder.Property(s => s.Buchholz)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(s => s.MedianBuchholz)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(s => s.HeadToHeadScore)
            .IsRequired();

        builder.Property(s => s.Rank)
            .IsRequired();

        // Unique constraint: one standing per participant per stage
        builder.HasIndex(s => new { s.TournamentStageId, s.ParticipantId })
            .IsUnique();

        // Relationships
        builder.HasOne(s => s.TournamentStage)
            .WithMany(ts => ts.Standings)
            .HasForeignKey(s => s.TournamentStageId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(s => s.Participant)
            .WithMany()
            .HasForeignKey(s => s.ParticipantId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

// Made with Bob