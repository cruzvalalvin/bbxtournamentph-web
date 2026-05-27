using BBXTournament.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BBXTournament.Infrastructure.Data.Configurations;

public class MatchConfiguration : IEntityTypeConfiguration<Match>
{
    public void Configure(EntityTypeBuilder<Match> builder)
    {
        builder.ToTable("Matches");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.TournamentStageId)
            .IsRequired();

        builder.Property(m => m.RoundNumber)
            .IsRequired();

        builder.Property(m => m.MatchNumber)
            .IsRequired();

        builder.Property(m => m.Player1Id);

        builder.Property(m => m.Player2Id);

        builder.Property(m => m.WinnerId);

        builder.Property(m => m.LoserId);

        builder.Property(m => m.Score1);

        builder.Property(m => m.Score2);

        builder.Property(m => m.IsBye)
            .IsRequired();

        builder.Property(m => m.Status)
            .HasConversion<string>()
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(m => m.CreatedAt)
            .IsRequired();

        // Relationships
        builder.HasOne(m => m.TournamentStage)
            .WithMany(ts => ts.Matches)
            .HasForeignKey(m => m.TournamentStageId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(m => m.Player1)
            .WithMany()
            .HasForeignKey(m => m.Player1Id)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(m => m.Player2)
            .WithMany()
            .HasForeignKey(m => m.Player2Id)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(m => m.Winner)
            .WithMany()
            .HasForeignKey(m => m.WinnerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(m => m.Loser)
            .WithMany()
            .HasForeignKey(m => m.LoserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

// Made with Bob