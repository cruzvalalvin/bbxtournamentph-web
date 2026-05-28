using BBXTournament.Domain.Entities;

namespace BBXTournament.Application.Interfaces.Tournaments;

public interface ITournamentRepository
{
    Task<Tournament?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Tournament?> GetForUpdateAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<Tournament>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<List<Tournament>> GetByCommunityIdAsync(Guid communityId, CancellationToken cancellationToken = default);
    Task<int> GetTournamentCountAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Tournament tournament, CancellationToken cancellationToken = default);
    Task<TournamentParticipant?> GetParticipantAsync(Guid tournamentId, Guid userId, CancellationToken cancellationToken = default);
    Task<TournamentParticipant?> GetParticipantByIdAsync(Guid participantId, CancellationToken cancellationToken = default);
    Task<List<TournamentParticipant>> GetParticipantsAsync(Guid tournamentId, CancellationToken cancellationToken = default);
    Task AddParticipantAsync(TournamentParticipant participant, CancellationToken cancellationToken = default);
    Task<TournamentStage?> GetStageAsync(Guid stageId, CancellationToken cancellationToken = default);
    Task<TournamentStage?> GetStageWithParticipantsAsync(Guid stageId, CancellationToken cancellationToken = default);
    Task<List<TournamentStage>> GetStagesAsync(Guid tournamentId, CancellationToken cancellationToken = default);
    Task AddStageAsync(TournamentStage stage, CancellationToken cancellationToken = default);
    Task<bool> StageOrderExistsAsync(Guid tournamentId, int stageOrder, CancellationToken cancellationToken = default);
    Task<TournamentRound?> GetRoundAsync(Guid roundId, CancellationToken cancellationToken = default);
    Task<List<TournamentRound>> GetRoundsByStageAsync(Guid stageId, CancellationToken cancellationToken = default);
    Task<TournamentRound?> GetRoundWithMatchesAsync(Guid roundId, CancellationToken cancellationToken = default);
    Task AddRoundAsync(TournamentRound round, CancellationToken cancellationToken = default);
    Task<List<Match>> GetMatchesByRoundAsync(Guid roundId, CancellationToken cancellationToken = default);
    Task<Match?> GetMatchByIdAsync(Guid matchId, CancellationToken cancellationToken = default);
    Task<Match?> GetMatchWithParticipantsAsync(Guid matchId, CancellationToken cancellationToken = default);
    Task AddMatchAsync(Match match, CancellationToken cancellationToken = default);
    Task AddMatchesAsync(List<Match> matches, CancellationToken cancellationToken = default);
    Task<List<TournamentParticipant>> GetStandingsAsync(Guid tournamentId, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}

// Made with Bob