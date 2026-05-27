using BBXTournament.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BBXTournament.Infrastructure.Data.Configurations;

public class TournamentParticipantConfiguration : IEntityTypeConfiguration<TournamentParticipant>
{
    public void Configure(EntityTypeBuilder<TournamentParticipant> builder)
    {
        builder.ToTable("TournamentParticipants");

        builder.HasKey(tp => tp.Id);

        builder.Property(tp => tp.TournamentId)
            .IsRequired();

        builder.Property(tp => tp.UserId);

        builder.Property(tp => tp.DisplayName)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(tp => tp.TeamName)
            .HasMaxLength(150);

        builder.Property(tp => tp.IsManualEntry)
            .IsRequired();

        builder.Property(tp => tp.IsPaid)
            .IsRequired();

        builder.Property(tp => tp.Seed);

        builder.Property(tp => tp.CheckedIn)
            .IsRequired();

        builder.Property(tp => tp.CreatedAt)
            .IsRequired();

        // Relationships
        builder.HasOne(tp => tp.Tournament)
            .WithMany(t => t.Participants)
            .HasForeignKey(tp => tp.TournamentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(tp => tp.User)
            .WithMany()
            .HasForeignKey(tp => tp.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

// Made with Bob