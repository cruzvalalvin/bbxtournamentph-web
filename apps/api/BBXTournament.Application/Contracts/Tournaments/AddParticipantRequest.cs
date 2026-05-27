namespace BBXTournament.Application.Contracts.Tournaments;

public class AddParticipantRequest
{
    public Guid? UserId { get; set; }
    public string DisplayName { get; set; } = string.Empty;
    public string? TeamName { get; set; }
    public bool IsManualEntry { get; set; }
    public int? Seed { get; set; }
}

// Made with Bob