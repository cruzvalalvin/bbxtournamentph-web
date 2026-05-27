using BBXTournament.Domain.Enums;

namespace BBXTournament.Domain.Entities;

public class TournamentRound
{
    public Guid Id { get; private set; }
    public Guid TournamentStageId { get; private set; }
    public int RoundNumber { get; private set; }
    public RoundStatus Status { get; private set; }
    public DateTime? StartedAt { get; private set; }
    public DateTime? CompletedAt { get; private set; }
    public DateTime CreatedAt { get; private set; }

    // Navigation properties
    public TournamentStage TournamentStage { get; private set; } = null!;
    public ICollection<Match> Matches { get; private set; } = new List<Match>();

    private TournamentRound()
    {
    }

    public TournamentRound(
        Guid tournamentStageId,
        int roundNumber)
    {
        Id = Guid.NewGuid();
        TournamentStageId = tournamentStageId;
        RoundNumber = roundNumber;
        Status = RoundStatus.Pending;
        CreatedAt = DateTime.UtcNow;
    }

    public void Start()
    {
        if (Status != RoundStatus.Pending)
        {
            throw new InvalidOperationException("Can only start a pending round.");
        }
        Status = RoundStatus.Ongoing;
        StartedAt = DateTime.UtcNow;
    }

    public void Complete()
    {
        if (Status != RoundStatus.Ongoing)
        {
            throw new InvalidOperationException("Can only complete an ongoing round.");
        }
        Status = RoundStatus.Completed;
        CompletedAt = DateTime.UtcNow;
    }

    public void Reset()
    {
        Status = RoundStatus.Pending;
        StartedAt = null;
        CompletedAt = null;
    }
}

// Made with Bob