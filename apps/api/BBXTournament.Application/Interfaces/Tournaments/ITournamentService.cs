using BBXTournament.Application.Contracts.Tournaments;

namespace BBXTournament.Application.Interfaces.Tournaments;

public interface ITournamentService
{
    Task<TournamentResponse> CreateTournamentAsync(CreateTournamentRequest request, Guid userId, CancellationToken cancellationToken = default);
    Task<List<TournamentResponse>> GetAllTournamentsAsync(CancellationToken cancellationToken = default);
    Task<TournamentResponse?> GetTournamentByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task CancelTournamentAsync(Guid id, Guid requesterId, string requesterRole, CancellationToken cancellationToken = default);
    Task DeleteTournamentAsync(Guid id, Guid requesterId, string requesterRole, CancellationToken cancellationToken = default);
    Task<ParticipantResponse> AddParticipantAsync(Guid tournamentId, AddParticipantRequest request, CancellationToken cancellationToken = default);
    Task<List<ParticipantResponse>> GetParticipantsAsync(Guid tournamentId, CancellationToken cancellationToken = default);
    Task CheckInParticipantAsync(Guid participantId, Guid requesterId, string requesterRole, CancellationToken cancellationToken = default);
    Task CheckOutParticipantAsync(Guid participantId, Guid requesterId, string requesterRole, CancellationToken cancellationToken = default);
    Task MarkParticipantAsPaidAsync(Guid participantId, Guid requesterId, string requesterRole, CancellationToken cancellationToken = default);
    Task MarkParticipantAsUnpaidAsync(Guid participantId, Guid requesterId, string requesterRole, CancellationToken cancellationToken = default);
    Task<StageResponse> CreateStageAsync(Guid tournamentId, CreateStageRequest request, CancellationToken cancellationToken = default);
    Task<List<StageResponse>> GetStagesAsync(Guid tournamentId, CancellationToken cancellationToken = default);
    Task<RoundResponse> GenerateRoundAsync(Guid stageId, CancellationToken cancellationToken = default);
    Task<List<RoundResponse>> GetRoundsByStageAsync(Guid stageId, CancellationToken cancellationToken = default);
    Task<List<MatchResponse>> GetMatchesByRoundAsync(Guid roundId, CancellationToken cancellationToken = default);
    Task<MatchResponse> SubmitMatchResultAsync(Guid matchId, SubmitMatchResultRequest request, Guid judgeUserId, CancellationToken cancellationToken = default);
    Task<List<StandingResponse>> GetTournamentStandingsAsync(Guid tournamentId, CancellationToken cancellationToken = default);
    Task<RoundResponse> ShuffleRoundAsync(Guid roundId, CancellationToken cancellationToken = default);
}

// Made with Bob