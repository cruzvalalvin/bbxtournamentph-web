using BBXTournament.Domain.Entities;

namespace BBXTournament.Application.Interfaces.Communities;

public interface ICommunityRepository
{
    Task<Community?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Community?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
    Task<List<Community>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<bool> SlugExistsAsync(string slug, CancellationToken cancellationToken = default);
    Task AddAsync(Community community, CancellationToken cancellationToken = default);
    Task<CommunityAdmin?> GetAdminAsync(Guid communityId, Guid userId, CancellationToken cancellationToken = default);
    Task AddAdminAsync(CommunityAdmin admin, CancellationToken cancellationToken = default);
    Task RemoveAdminAsync(CommunityAdmin admin, CancellationToken cancellationToken = default);
    Task<List<CommunityMember>> GetMembersAsync(Guid communityId, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}

// Made with Bob