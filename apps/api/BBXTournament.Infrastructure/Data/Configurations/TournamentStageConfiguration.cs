using BBXTournament.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BBXTournament.Infrastructure.Data.Configurations;

public class TournamentStageConfiguration : IEntityTypeConfiguration<TournamentStage>
{
    public void Configure(EntityTypeBuilder<TournamentStage> builder)
    {
        builder.ToTable("TournamentStages");

        builder.HasKey(ts => ts.Id);

        builder.Property(ts => ts.TournamentId)
            .IsRequired();

        builder.Property(ts => ts.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(ts => ts.StageOrder)
            .IsRequired();

        builder.Property(ts => ts.FormatType)
            .HasConversion<string>()
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(ts => ts.NumberOfRounds)
            .IsRequired();

        builder.Property(ts => ts.GroupCount);

        builder.Property(ts => ts.AdvanceCount);

        builder.Property(ts => ts.HasThirdPlaceMatch)
            .IsRequired();

        builder.Property(ts => ts.HasGrandFinalReset)
            .IsRequired();

        builder.Property(ts => ts.Status)
            .HasConversion<string>()
            .IsRequired()
            .HasMaxLength(50);

        // Unique constraint: stage order must be unique per tournament
        builder.HasIndex(ts => new { ts.TournamentId, ts.StageOrder })
            .IsUnique();

        // Relationships
        builder.HasOne(ts => ts.Tournament)
            .WithMany(t => t.Stages)
            .HasForeignKey(ts => ts.TournamentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(ts => ts.Matches)
            .WithOne(m => m.TournamentStage)
            .HasForeignKey(m => m.TournamentStageId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(ts => ts.Standings)
            .WithOne(s => s.TournamentStage)
            .HasForeignKey(s => s.TournamentStageId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

// Made with Bob