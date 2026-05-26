using BBXTournament.Domain.Enums;

namespace BBXTournament.Application.Contracts.Communities;

public class AddCommunityAdminRequest
{
    public Guid UserId { get; set; }
    public CommunityRole Role { get; set; }
}

// Made with Bob