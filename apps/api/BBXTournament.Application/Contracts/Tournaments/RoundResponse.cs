using BBXTournament.Domain.Enums;

namespace BBXTournament.Application.Contracts.Tournaments;

public class RoundResponse
{
    public Guid Id { get; set; }
    public Guid TournamentStageId { get; set; }
    public int RoundNumber { get; set; }
    public RoundStatus Status { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<MatchResponse> Matches { get; set; } = new();
}

// Made with Bob