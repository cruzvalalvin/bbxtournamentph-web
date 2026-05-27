using BBXTournament.Domain.Enums;

namespace BBXTournament.Domain.Entities;

public class Match
{
    public Guid Id { get; private set; }
    public Guid TournamentStageId { get; private set; }
    public Guid TournamentRoundId { get; private set; }
    public int RoundNumber { get; private set; }
    public int MatchNumber { get; private set; }
    public Guid? Player1Id { get; private set; }
    public Guid? Player2Id { get; private set; }
    public int? Player1Score { get; private set; }
    public int? Player2Score { get; private set; }
    public Guid? WinnerParticipantId { get; private set; }
    public Guid? LoserParticipantId { get; private set; }
    public Guid? JudgeUserId { get; private set; }
    public bool IsBye { get; private set; }
    public MatchStatus Status { get; private set; }
    public DateTime? CompletedAt { get; private set; }
    public DateTime CreatedAt { get; private set; }

    // Navigation properties
    public TournamentStage TournamentStage { get; private set; } = null!;
    public TournamentRound TournamentRound { get; private set; } = null!;
    public TournamentParticipant? Player1 { get; private set; }
    public TournamentParticipant? Player2 { get; private set; }
    public TournamentParticipant? WinnerParticipant { get; private set; }
    public TournamentParticipant? LoserParticipant { get; private set; }
    public User? JudgeUser { get; private set; }

    private Match()
    {
    }

    public Match(
        Guid tournamentStageId,
        Guid tournamentRoundId,
        int roundNumber,
        int matchNumber,
        Guid? player1Id = null,
        Guid? player2Id = null,
        bool isBye = false)
    {
        Id = Guid.NewGuid();
        TournamentStageId = tournamentStageId;
        TournamentRoundId = tournamentRoundId;
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

    public void ReportScore(int score1, int score2, Guid? judgeUserId = null)
    {
        if (Status == MatchStatus.Completed)
        {
            throw new InvalidOperationException("Cannot report score for a completed match.");
        }
        if (!Player1Id.HasValue || !Player2Id.HasValue)
        {
            throw new InvalidOperationException("Both players must be assigned before reporting score.");
        }

        Player1Score = score1;
        Player2Score = score2;
        JudgeUserId = judgeUserId;

        if (score1 > score2)
        {
            WinnerParticipantId = Player1Id;
            LoserParticipantId = Player2Id;
        }
        else if (score2 > score1)
        {
            WinnerParticipantId = Player2Id;
            LoserParticipantId = Player1Id;
        }
        else
        {
            // Draw - no winner/loser
            WinnerParticipantId = null;
            LoserParticipantId = null;
        }

        Status = MatchStatus.Completed;
        CompletedAt = DateTime.UtcNow;
    }

    public void MarkAsBye(Guid winnerParticipantId)
    {
        if (!IsBye)
        {
            throw new InvalidOperationException("This match is not marked as a bye.");
        }
        WinnerParticipantId = winnerParticipantId;
        Status = MatchStatus.Completed;
        CompletedAt = DateTime.UtcNow;
    }

    public void Reset()
    {
        Player1Score = null;
        Player2Score = null;
        WinnerParticipantId = null;
        LoserParticipantId = null;
        JudgeUserId = null;
        CompletedAt = null;
        Status = MatchStatus.Pending;
    }
}

// Made with Bob