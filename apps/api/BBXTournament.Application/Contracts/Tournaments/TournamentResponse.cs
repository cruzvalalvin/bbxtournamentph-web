using BBXTournament.Domain.Enums;

namespace BBXTournament.Application.Contracts.Tournaments;

public class TournamentResponse
{
    public Guid Id { get; set; }
    public string PublicCode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public TournamentStatus Status { get; set; }
    public int MaxParticipants { get; set; }
    public Guid CommunityId { get; set; }
    public string CommunityName { get; set; } = string.Empty;
    public string? Region { get; set; }
    public string? Province { get; set; }
    public string? City { get; set; }
    public DateTime CreatedAt { get; set; }
}

// Made with Bob