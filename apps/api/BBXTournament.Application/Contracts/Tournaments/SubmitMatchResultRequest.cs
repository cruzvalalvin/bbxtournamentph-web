namespace BBXTournament.Application.Contracts.Tournaments;

public record SubmitMatchResultRequest(
    int Player1Score,
    int Player2Score,
    string? JudgeNotes = null
);

// Made with Bob