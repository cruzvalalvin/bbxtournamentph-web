namespace BBXTournament.Domain.Entities;

public class TournamentParticipant
{
    public Guid Id { get; private set; }
    public Guid TournamentId { get; private set; }
    public Guid? UserId { get; private set; }
    public string DisplayName { get; private set; }
    public string? TeamName { get; private set; }
    public bool IsManualEntry { get; private set; }
    public bool IsPaid { get; private set; }
    public int? Seed { get; private set; }
    public bool CheckedIn { get; private set; }
    public DateTime CreatedAt { get; private set; }

    // Navigation properties
    public Tournament Tournament { get; private set; } = null!;
    public User? User { get; private set; }

    private TournamentParticipant()
    {
        DisplayName = string.Empty;
    }

    public TournamentParticipant(
        Guid tournamentId,
        Guid? userId,
        string displayName,
        string? teamName = null,
        bool isManualEntry = false,
        int? seed = null)
    {
        Id = Guid.NewGuid();
        TournamentId = tournamentId;
        UserId = userId;
        DisplayName = displayName.Trim();
        TeamName = string.IsNullOrWhiteSpace(teamName) ? null : teamName.Trim();
        IsManualEntry = isManualEntry;
        IsPaid = false;
        Seed = seed;
        CheckedIn = false;
        CreatedAt = DateTime.UtcNow;
    }

    public void Update(
        string displayName,
        string? teamName,
        int? seed)
    {
        DisplayName = displayName.Trim();
        TeamName = string.IsNullOrWhiteSpace(teamName) ? null : teamName.Trim();
        Seed = seed;
    }

    public void MarkAsPaid()
    {
        IsPaid = true;
    }

    public void CheckIn()
    {
        CheckedIn = true;
    }

    public void UndoCheckIn()
    {
        CheckedIn = false;
    }

    public void UpdateSeed(int seed)
    {
        Seed = seed;
    }
}

// Made with Bob