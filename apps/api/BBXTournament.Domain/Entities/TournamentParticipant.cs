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

    // Match statistics (Beyblade X scoring)
    public int MatchWins { get; private set; }
    public int MatchLosses { get; private set; }
    public int PointsScored { get; private set; }
    public int PointsAgainst { get; private set; }
    public int PointsDifference { get; private set; }
    public decimal BuchholzScore { get; private set; }

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
        
        // Initialize statistics
        MatchWins = 0;
        MatchLosses = 0;
        PointsScored = 0;
        PointsAgainst = 0;
        PointsDifference = 0;
        BuchholzScore = 0;
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

    public void UnmarkAsPaid()
    {
        IsPaid = false;
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

    public void RecordWin(int pointsScored, int pointsAgainst)
    {
        MatchWins++;
        PointsScored += pointsScored;
        PointsAgainst += pointsAgainst;
        PointsDifference = PointsScored - PointsAgainst;
    }

    public void RecordLoss(int pointsScored, int pointsAgainst)
    {
        MatchLosses++;
        PointsScored += pointsScored;
        PointsAgainst += pointsAgainst;
        PointsDifference = PointsScored - PointsAgainst;
    }

    public void UpdateBuchholzScore(decimal buchholzScore)
    {
        BuchholzScore = buchholzScore;
    }
}

// Made with Bob