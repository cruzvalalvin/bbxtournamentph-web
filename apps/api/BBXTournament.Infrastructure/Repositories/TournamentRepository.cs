using BBXTournament.Application.Interfaces.Tournaments;
using BBXTournament.Domain.Entities;
using BBXTournament.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BBXTournament.Infrastructure.Repositories;

public class TournamentRepository : ITournamentRepository
{
    private readonly ApplicationDbContext _dbContext;

    public TournamentRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Tournament?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Tournaments
            .Include(t => t.Community)
            .Include(t => t.CreatedBy)
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public async Task<Tournament?> GetForUpdateAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Tournaments
            .Include(t => t.Community)
            .Include(t => t.CreatedBy)
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public async Task<List<Tournament>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Tournaments
            .Include(t => t.Community)
            .Include(t => t.CreatedBy)
            .AsNoTracking()
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Tournament>> GetByCommunityIdAsync(Guid communityId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Tournaments
            .Include(t => t.Community)
            .Include(t => t.CreatedBy)
            .Where(t => t.CommunityId == communityId)
            .AsNoTracking()
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetTournamentCountAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Tournaments.CountAsync(cancellationToken);
    }

    public async Task AddAsync(Tournament tournament, CancellationToken cancellationToken = default)
    {
        await _dbContext.Tournaments.AddAsync(tournament, cancellationToken);
    }

    public async Task<TournamentParticipant?> GetParticipantAsync(Guid tournamentId, Guid userId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.TournamentParticipants
            .FirstOrDefaultAsync(p => p.TournamentId == tournamentId && p.UserId == userId, cancellationToken);
    }

    public async Task<List<TournamentParticipant>> GetParticipantsAsync(Guid tournamentId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.TournamentParticipants
            .Include(p => p.User)
            .Where(p => p.TournamentId == tournamentId)
            .AsNoTracking()
            .OrderBy(p => p.Seed)
            .ThenBy(p => p.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task AddParticipantAsync(TournamentParticipant participant, CancellationToken cancellationToken = default)
    {
        await _dbContext.TournamentParticipants.AddAsync(participant, cancellationToken);
    }

    public async Task<TournamentStage?> GetStageAsync(Guid stageId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.TournamentStages
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == stageId, cancellationToken);
    }

    public async Task<List<TournamentStage>> GetStagesAsync(Guid tournamentId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.TournamentStages
            .Where(s => s.TournamentId == tournamentId)
            .AsNoTracking()
            .OrderBy(s => s.StageOrder)
            .ToListAsync(cancellationToken);
    }

    public async Task AddStageAsync(TournamentStage stage, CancellationToken cancellationToken = default)
    {
        await _dbContext.TournamentStages.AddAsync(stage, cancellationToken);
    }

    public async Task<bool> StageOrderExistsAsync(Guid tournamentId, int stageOrder, CancellationToken cancellationToken = default)
    {
        return await _dbContext.TournamentStages
            .AnyAsync(s => s.TournamentId == tournamentId && s.StageOrder == stageOrder, cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}

// Made with Bob