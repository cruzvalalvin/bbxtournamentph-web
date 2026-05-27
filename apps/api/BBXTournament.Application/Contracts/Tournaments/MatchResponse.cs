using BBXTournament.Domain.Enums;

namespace BBXTournament.Application.Contracts.Tournaments;

public class MatchResponse
{
    public Guid Id { get; set; }
    public Guid TournamentStageId { get; set; }
    public Guid TournamentRoundId { get; set; }
    public int RoundNumber { get; set; }
    public int MatchNumber { get; set; }
    public Guid? Player1Id { get; set; }
    public string? Player1DisplayName { get; set; }
    public Guid? Player2Id { get; set; }
    public string? Player2DisplayName { get; set; }
    public int? Player1Score { get; set; }
    public int? Player2Score { get; set; }
    public Guid? WinnerParticipantId { get; set; }
    public string? WinnerDisplayName { get; set; }
    public Guid? LoserParticipantId { get; set; }
    public Guid? JudgeUserId { get; set; }
    public bool IsBye { get; set; }
    public MatchStatus Status { get; set; }
    public DateTime? CompletedAt { get; set; }
    public DateTime CreatedAt { get; set; }
}

// Made with Bob