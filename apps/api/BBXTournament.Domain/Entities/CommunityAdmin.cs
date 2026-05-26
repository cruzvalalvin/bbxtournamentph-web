using BBXTournament.Domain.Enums;

namespace BBXTournament.Domain.Entities;

public class CommunityAdmin
{
    public Guid Id { get; private set; }
    public Guid CommunityId { get; private set; }
    public Guid UserId { get; private set; }
    public CommunityRole Role { get; private set; }
    public DateTime CreatedAt { get; private set; }

    // Navigation properties
    public Community Community { get; private set; } = null!;
    public User User { get; private set; } = null!;

    private CommunityAdmin()
    {
    }

    public CommunityAdmin(
        Guid communityId,
        Guid userId,
        CommunityRole role)
    {
        Id = Guid.NewGuid();
        CommunityId = communityId;
        UserId = userId;
        Role = role;
        CreatedAt = DateTime.UtcNow;
    }

    public void UpdateRole(CommunityRole role)
    {
        Role = role;
    }
}

// Made with Bob