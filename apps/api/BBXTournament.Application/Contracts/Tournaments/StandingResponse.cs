namespace BBXTournament.Application.Contracts.Tournaments;

public record StandingResponse(
    Guid ParticipantId,
    string DisplayName,
    string? TeamName,
    int Rank,
    int MatchWins,
    int MatchLosses,
    int PointsScored,
    int PointsAgainst,
    int PointsDifference,
    decimal BuchholzScore
);

// Made with Bob