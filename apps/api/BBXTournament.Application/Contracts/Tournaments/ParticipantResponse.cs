namespace BBXTournament.Application.Contracts.Tournaments;

public class ParticipantResponse
{
    public Guid Id { get; set; }
    public Guid TournamentId { get; set; }
    public Guid? UserId { get; set; }
    public string DisplayName { get; set; } = string.Empty;
    public string? TeamName { get; set; }
    public bool IsManualEntry { get; set; }
    public bool IsPaid { get; set; }
    public int? Seed { get; set; }
    public bool CheckedIn { get; set; }
    public DateTime CreatedAt { get; set; }
}

// Made with Bob