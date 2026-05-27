namespace BBXTournament.Application.Contracts.Communities;

public class CommunityResponse
{
    public Guid Id { get; set; }
    public string PublicCode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Region { get; set; }
    public string? Province { get; set; }
    public string? City { get; set; }
    public bool IsVerified { get; set; }
}

// Made with Bob