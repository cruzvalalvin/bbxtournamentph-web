using BBXTournament.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BBXTournament.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Community> Communities => Set<Community>();
    public DbSet<CommunityAdmin> CommunityAdmins => Set<CommunityAdmin>();
    public DbSet<CommunityMember> CommunityMembers => Set<CommunityMember>();
    public DbSet<Tournament> Tournaments => Set<Tournament>();
    public DbSet<TournamentStage> TournamentStages => Set<TournamentStage>();
    public DbSet<TournamentParticipant> TournamentParticipants => Set<TournamentParticipant>();
    public DbSet<Match> Matches => Set<Match>();
    public DbSet<Standing> Standings => Set<Standing>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}

// Made with Bob
