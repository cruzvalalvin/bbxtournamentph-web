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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}

// Made with Bob
