namespace BBXTournament.Application.Contracts.Tournaments;

public class CreateTournamentRequest
{
    public Guid CommunityId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int MaxParticipants { get; set; }
    public string? Region { get; set; }
    public string? Province { get; set; }
    public string? City { get; set; }
}

// Made with Bob