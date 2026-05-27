using BBXTournament.Domain.Enums;

namespace BBXTournament.Domain.Entities;

public class Match
{
    public Guid Id { get; private set; }
    public Guid TournamentStageId { get; private set; }
    public int RoundNumber { get; private set; }
    public int MatchNumber { get; private set; }
    public Guid? Player1Id { get; private set; }
    public Guid? Player2Id { get; private set; }
    public Guid? WinnerId { get; private set; }
    public Guid? LoserId { get; private set; }
    public int? Score1 { get; private set; }
    public int? Score2 { get; private set; }
    public bool IsBye { get; private set; }
    public MatchStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }

    // Navigation properties
    public TournamentStage TournamentStage { get; private set; } = null!;
    public TournamentParticipant? Player1 { get; private set; }
    public TournamentParticipant? Player2 { get; private set; }
    public TournamentParticipant? Winner { get; private set; }
    public TournamentParticipant? Loser { get; private set; }

    private Match()
    {
    }

    public Match(
        Guid tournamentStageId,
        int roundNumber,
        int matchNumber,
        Guid? player1Id = null,
        Guid? player2Id = null,
        bool isBye = false)
    {
        Id = Guid.NewGuid();
        TournamentStageId = tournamentStageId;
        RoundNumber = roundNumber;
        MatchNumber = matchNumber;
        Player1Id = player1Id;
        Player2Id = player2Id;
        IsBye = isBye;
        Status = MatchStatus.Pending;
        CreatedAt = DateTime.UtcNow;
    }

    public void AssignPlayer1(Guid participantId)
    {
        if (Status != MatchStatus.Pending)
        {
            throw new InvalidOperationException("Cannot assign players to a match that is not pending.");
        }
        Player1Id = participantId;
    }

    public void AssignPlayer2(Guid participantId)
    {
        if (Status != MatchStatus.Pending)
        {
            throw new InvalidOperationException("Cannot assign players to a match that is not pending.");
        }
        Player2Id = participantId;
    }

    public void Start()
    {
        if (Status != MatchStatus.Pending)
        {
            throw new InvalidOperationException("Can only start a pending match.");
        }
        if (!Player1Id.HasValue || !Player2Id.HasValue)
        {
            throw new InvalidOperationException("Both players must be assigned before starting the match.");
        }
        Status = MatchStatus.Ongoing;
    }

    public void ReportScore(int score1, int score2)
    {
        if (Status == MatchStatus.Completed)
        {
            throw new InvalidOperationException("Cannot report score for a completed match.");
        }
        if (!Player1Id.HasValue || !Player2Id.HasValue)
        {
            throw new InvalidOperationException("Both players must be assigned before reporting score.");
        }

        Score1 = score1;
        Score2 = score2;

        if (score1 > score2)
        {
            WinnerId = Player1Id;
            LoserId = Player2Id;
        }
        else if (score2 > score1)
        {
            WinnerId = Player2Id;
            LoserId = Player1Id;
        }
        else
        {
            // Draw - no winner/loser
            WinnerId = null;
            LoserId = null;
        }

        Status = MatchStatus.Completed;
    }

    public void MarkAsBye(Guid winnerId)
    {
        if (!IsBye)
        {
            throw new InvalidOperationException("This match is not marked as a bye.");
        }
        WinnerId = winnerId;
        Status = MatchStatus.Completed;
    }

    public void Reset()
    {
        Score1 = null;
        Score2 = null;
        WinnerId = null;
        LoserId = null;
        Status = MatchStatus.Pending;
    }
}

// Made with Bob