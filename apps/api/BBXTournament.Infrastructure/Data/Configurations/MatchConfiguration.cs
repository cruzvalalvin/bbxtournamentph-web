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

        builder.Property(m => m.TournamentRoundId)
            .IsRequired();

        builder.Property(m => m.RoundNumber)
            .IsRequired();

        builder.Property(m => m.MatchNumber)
            .IsRequired();

        builder.Property(m => m.Player1Id);

        builder.Property(m => m.Player2Id);

        builder.Property(m => m.Player1Score);

        builder.Property(m => m.Player2Score);

        builder.Property(m => m.WinnerParticipantId);

        builder.Property(m => m.LoserParticipantId);

        builder.Property(m => m.JudgeUserId);

        builder.Property(m => m.JudgeNotes)
            .HasMaxLength(1000);

        builder.Property(m => m.IsBye)
            .IsRequired();

        builder.Property(m => m.Status)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(m => m.CompletedAt);

        builder.Property(m => m.CreatedAt)
            .IsRequired();

        // Relationships
        builder.HasOne(m => m.TournamentStage)
            .WithMany(ts => ts.Matches)
            .HasForeignKey(m => m.TournamentStageId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(m => m.TournamentRound)
            .WithMany(r => r.Matches)
            .HasForeignKey(m => m.TournamentRoundId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(m => m.Player1)
            .WithMany()
            .HasForeignKey(m => m.Player1Id)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(m => m.Player2)
            .WithMany()
            .HasForeignKey(m => m.Player2Id)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(m => m.WinnerParticipant)
            .WithMany()
            .HasForeignKey(m => m.WinnerParticipantId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(m => m.LoserParticipant)
            .WithMany()
            .HasForeignKey(m => m.LoserParticipantId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(m => m.JudgeUser)
            .WithMany()
            .HasForeignKey(m => m.JudgeUserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(m => m.TournamentStageId);
        builder.HasIndex(m => m.TournamentRoundId);
    }
}

// Made with Bob