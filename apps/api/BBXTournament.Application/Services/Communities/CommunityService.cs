using BBXTournament.Application.Contracts.Auth;
using BBXTournament.Application.Contracts.Communities;
using BBXTournament.Application.Interfaces.Communities;
using BBXTournament.Domain.Entities;
using BBXTournament.Domain.Enums;

namespace BBXTournament.Application.Services.Communities;

public class CommunityService : ICommunityService
{
    private readonly ICommunityRepository _communityRepository;

    public CommunityService(ICommunityRepository communityRepository)
    {
        _communityRepository = communityRepository;
    }

    public async Task<CommunityResponse> CreateCommunityAsync(
        CreateCommunityRequest request,
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        // Validate slug uniqueness
        if (await _communityRepository.SlugExistsAsync(request.Slug, cancellationToken))
        {
            throw new InvalidOperationException("Community slug already exists.");
        }

        var community = new Community(
            request.Name,
            request.Slug,
            request.Description,
            userId,
            request.LogoUrl,
            request.BannerUrl,
            request.Region,
            request.Province,
            request.City);

        await _communityRepository.AddAsync(community, cancellationToken);
        await _communityRepository.SaveChangesAsync(cancellationToken);

        // Reload with owner
        var created = await _communityRepository.GetByIdAsync(community.Id, cancellationToken);
        return MapToCommunityResponse(created!);
    }

    public async Task<List<CommunityResponse>> GetAllCommunitiesAsync(CancellationToken cancellationToken = default)
    {
        var communities = await _communityRepository.GetAllAsync(cancellationToken);
        return communities.Select(MapToCommunityResponse).ToList();
    }

    public async Task<CommunityResponse?> GetCommunityByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var community = await _communityRepository.GetByIdAsync(id, cancellationToken);
        return community is null ? null : MapToCommunityResponse(community);
    }

    public async Task AddAdminAsync(
        Guid communityId,
        AddCommunityAdminRequest request,
        Guid requesterId,
        CancellationToken cancellationToken = default)
    {
        var community = await _communityRepository.GetByIdAsync(communityId, cancellationToken);
        if (community is null)
        {
            throw new InvalidOperationException("Community not found.");
        }

        // Only owner can add admins
        if (community.OwnerId != requesterId)
        {
            throw new UnauthorizedAccessException("Only the community owner can add admins.");
        }

        // Check if user is already an admin
        var existingAdmin = await _communityRepository.GetAdminAsync(communityId, request.UserId, cancellationToken);
        if (existingAdmin is not null)
        {
            throw new InvalidOperationException("User is already an admin of this community.");
        }

        var admin = new CommunityAdmin(communityId, request.UserId, request.Role);
        await _communityRepository.AddAdminAsync(admin, cancellationToken);
        await _communityRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveAdminAsync(
        Guid communityId,
        Guid userId,
        Guid requesterId,
        CancellationToken cancellationToken = default)
    {
        var community = await _communityRepository.GetByIdAsync(communityId, cancellationToken);
        if (community is null)
        {
            throw new InvalidOperationException("Community not found.");
        }

        // Only owner can remove admins
        if (community.OwnerId != requesterId)
        {
            throw new UnauthorizedAccessException("Only the community owner can remove admins.");
        }

        var admin = await _communityRepository.GetAdminAsync(communityId, userId, cancellationToken);
        if (admin is null)
        {
            throw new InvalidOperationException("Admin not found.");
        }

        await _communityRepository.RemoveAdminAsync(admin, cancellationToken);
        await _communityRepository.SaveChangesAsync(cancellationToken);
    }

    private static CommunityResponse MapToCommunityResponse(Community community)
    {
        return new CommunityResponse
        {
            Id = community.Id,
            Name = community.Name,
            Slug = community.Slug,
            Description = community.Description,
            LogoUrl = community.LogoUrl,
            BannerUrl = community.BannerUrl,
            Region = community.Region,
            Province = community.Province,
            City = community.City,
            OwnerId = community.OwnerId,
            Owner = community.Owner is not null ? new UserResponse
            {
                Id = community.Owner.Id,
                DisplayName = community.Owner.DisplayName,
                Email = community.Owner.Email,
                Role = community.Owner.Role,
                Region = community.Owner.Region,
                Province = community.Owner.Province,
                City = community.Owner.City,
                IsActive = community.Owner.IsActive,
                CreatedAt = community.Owner.CreatedAt
            } : null,
            IsVerified = community.IsVerified,
            IsActive = community.IsActive,
            CreatedAt = community.CreatedAt,
            UpdatedAt = community.UpdatedAt
        };
    }
}

// Made with Bob