using BBXTournament.Application.Contracts.Communities;

namespace BBXTournament.Application.Interfaces.Communities;

public interface ICommunityService
{
    Task<CommunityResponse> CreateCommunityAsync(CreateCommunityRequest request, Guid userId, CancellationToken cancellationToken = default);
    Task<List<CommunityResponse>> GetAllCommunitiesAsync(CancellationToken cancellationToken = default);
    Task<CommunityResponse?> GetCommunityByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAdminAsync(Guid communityId, AddCommunityAdminRequest request, Guid requesterId, CancellationToken cancellationToken = default);
    Task RemoveAdminAsync(Guid communityId, Guid userId, Guid requesterId, CancellationToken cancellationToken = default);
}

// Made with Bob