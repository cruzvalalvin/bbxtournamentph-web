using BBXTournament.Application.Interfaces.Communities;
using BBXTournament.Domain.Entities;
using BBXTournament.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BBXTournament.Infrastructure.Repositories;

public class CommunityRepository : ICommunityRepository
{
    private readonly ApplicationDbContext _dbContext;

    public CommunityRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Community?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Communities
            .Include(c => c.Owner)
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<Community?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Communities
            .Include(c => c.Owner)
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Slug == slug.ToLowerInvariant(), cancellationToken);
    }

    public async Task<List<Community>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Communities
            .Include(c => c.Owner)
            .AsNoTracking()
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> SlugExistsAsync(string slug, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Communities
            .AnyAsync(c => c.Slug == slug.ToLowerInvariant(), cancellationToken);
    }

    public async Task AddAsync(Community community, CancellationToken cancellationToken = default)
    {
        await _dbContext.Communities.AddAsync(community, cancellationToken);
    }

    public async Task<CommunityAdmin?> GetAdminAsync(Guid communityId, Guid userId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.CommunityAdmins
            .FirstOrDefaultAsync(ca => ca.CommunityId == communityId && ca.UserId == userId, cancellationToken);
    }

    public async Task AddAdminAsync(CommunityAdmin admin, CancellationToken cancellationToken = default)
    {
        await _dbContext.CommunityAdmins.AddAsync(admin, cancellationToken);
    }

    public Task RemoveAdminAsync(CommunityAdmin admin, CancellationToken cancellationToken = default)
    {
        _dbContext.CommunityAdmins.Remove(admin);
        return Task.CompletedTask;
    }

    public async Task<List<CommunityMember>> GetMembersAsync(Guid communityId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.CommunityMembers
            .Include(cm => cm.User)
            .Where(cm => cm.CommunityId == communityId)
            .AsNoTracking()
            .OrderByDescending(cm => cm.JoinedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}

// Made with Bob