namespace BBXTournament.Domain.Entities;

public class CommunityMember
{
    public Guid Id { get; private set; }
    public Guid CommunityId { get; private set; }
    public Guid UserId { get; private set; }
    public DateTime JoinedAt { get; private set; }

    // Navigation properties
    public Community Community { get; private set; } = null!;
    public User User { get; private set; } = null!;

    private CommunityMember()
    {
    }

    public CommunityMember(
        Guid communityId,
        Guid userId)
    {
        Id = Guid.NewGuid();
        CommunityId = communityId;
        UserId = userId;
        JoinedAt = DateTime.UtcNow;
    }
}

// Made with Bob