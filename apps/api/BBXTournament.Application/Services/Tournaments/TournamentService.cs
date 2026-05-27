using BBXTournament.Application.Contracts.Auth;
using BBXTournament.Application.Contracts.Communities;
using BBXTournament.Application.Contracts.Tournaments;
using BBXTournament.Application.Interfaces.Tournaments;
using BBXTournament.Domain.Entities;
using BBXTournament.Domain.Enums;

namespace BBXTournament.Application.Services.Tournaments;

public class TournamentService : ITournamentService
{
    private readonly ITournamentRepository _tournamentRepository;

    public TournamentService(ITournamentRepository tournamentRepository)
    {
        _tournamentRepository = tournamentRepository;
    }

    public async Task<TournamentResponse> CreateTournamentAsync(
        CreateTournamentRequest request,
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var tournament = new Tournament(
            request.CommunityId,
            request.Name,
            request.Description,
            request.MaxParticipants,
            userId,
            request.Region,
            request.Province,
            request.City);

        await _tournamentRepository.AddAsync(tournament, cancellationToken);
        await _tournamentRepository.SaveChangesAsync(cancellationToken);

        // Generate PublicCode after save to ensure we have the ID
        var count = await _tournamentRepository.GetTournamentCountAsync(cancellationToken);
        var publicCode = $"TRN-{count:D4}";
        tournament.SetPublicCode(publicCode);
        await _tournamentRepository.SaveChangesAsync(cancellationToken);

        var created = await _tournamentRepository.GetByIdAsync(tournament.Id, cancellationToken);
        return MapToTournamentResponse(created!);
    }

    public async Task<List<TournamentResponse>> GetAllTournamentsAsync(CancellationToken cancellationToken = default)
    {
        var tournaments = await _tournamentRepository.GetAllAsync(cancellationToken);
        return tournaments.Select(MapToTournamentResponse).ToList();
    }

    public async Task<TournamentResponse?> GetTournamentByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var tournament = await _tournamentRepository.GetByIdAsync(id, cancellationToken);
        return tournament is null ? null : MapToTournamentResponse(tournament);
    }

    public async Task CancelTournamentAsync(
        Guid id,
        Guid requesterId,
        string requesterRole,
        CancellationToken cancellationToken = default)
    {
        var tournament = await _tournamentRepository.GetForUpdateAsync(id, cancellationToken);
        if (tournament is null)
        {
            throw new InvalidOperationException("Tournament not found.");
        }

        if (!CanCancelTournament(tournament, requesterId, requesterRole))
        {
            throw new UnauthorizedAccessException("Only the community owner or super admin can cancel this tournament.");
        }

        tournament.Cancel();
        await _tournamentRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteTournamentAsync(
        Guid id,
        Guid requesterId,
        string requesterRole,
        CancellationToken cancellationToken = default)
    {
        var tournament = await _tournamentRepository.GetForUpdateAsync(id, cancellationToken);
        if (tournament is null)
        {
            throw new InvalidOperationException("Tournament not found.");
        }

        if (!string.Equals(requesterRole, UserRole.SuperAdmin.ToString(), StringComparison.OrdinalIgnoreCase))
        {
            throw new UnauthorizedAccessException("Only super admin can delete tournaments.");
        }

        tournament.Delete();
        await _tournamentRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task<ParticipantResponse> AddParticipantAsync(
        Guid tournamentId,
        AddParticipantRequest request,
        CancellationToken cancellationToken = default)
    {
        var tournament = await _tournamentRepository.GetByIdAsync(tournamentId, cancellationToken);
        if (tournament is null)
        {
            throw new InvalidOperationException("Tournament not found.");
        }

        var participants = await _tournamentRepository.GetParticipantsAsync(tournamentId, cancellationToken);
        if (participants.Count >= tournament.MaxParticipants)
        {
            throw new InvalidOperationException("Tournament is full.");
        }

        if (request.UserId.HasValue)
        {
            var existing = await _tournamentRepository.GetParticipantAsync(tournamentId, request.UserId.Value, cancellationToken);
            if (existing is not null)
            {
                throw new InvalidOperationException("User is already registered for this tournament.");
            }
        }

        var participant = new TournamentParticipant(
            tournamentId,
            request.UserId,
            request.DisplayName,
            request.TeamName,
            request.IsManualEntry,
            request.Seed);

        await _tournamentRepository.AddParticipantAsync(participant, cancellationToken);
        await _tournamentRepository.SaveChangesAsync(cancellationToken);

        return MapToParticipantResponse(participant);
    }

    public async Task<List<ParticipantResponse>> GetParticipantsAsync(Guid tournamentId, CancellationToken cancellationToken = default)
    {
        var participants = await _tournamentRepository.GetParticipantsAsync(tournamentId, cancellationToken);
        return participants.Select(MapToParticipantResponse).ToList();
    }

    public async Task<StageResponse> CreateStageAsync(
        Guid tournamentId,
        CreateStageRequest request,
        CancellationToken cancellationToken = default)
    {
        var tournament = await _tournamentRepository.GetByIdAsync(tournamentId, cancellationToken);
        if (tournament is null)
        {
            throw new InvalidOperationException("Tournament not found.");
        }

        if (await _tournamentRepository.StageOrderExistsAsync(tournamentId, request.StageOrder, cancellationToken))
        {
            throw new InvalidOperationException("Stage order already exists for this tournament.");
        }

        var stage = new TournamentStage(
            tournamentId,
            request.Name,
            request.StageOrder,
            request.FormatType,
            request.NumberOfRounds,
            request.GroupCount,
            request.AdvanceCount,
            request.HasThirdPlaceMatch,
            request.HasGrandFinalReset);

        await _tournamentRepository.AddStageAsync(stage, cancellationToken);
        await _tournamentRepository.SaveChangesAsync(cancellationToken);

        return MapToStageResponse(stage);
    }

    public async Task<List<StageResponse>> GetStagesAsync(Guid tournamentId, CancellationToken cancellationToken = default)
    {
        var stages = await _tournamentRepository.GetStagesAsync(tournamentId, cancellationToken);
        return stages.Select(MapToStageResponse).ToList();
    }

    private static bool CanCancelTournament(Tournament tournament, Guid requesterId, string requesterRole)
    {
        if (string.Equals(requesterRole, UserRole.SuperAdmin.ToString(), StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        return tournament.Community.OwnerId == requesterId;
    }

    private static TournamentResponse MapToTournamentResponse(Tournament tournament)
    {
        return new TournamentResponse
        {
            Id = tournament.Id,
            PublicCode = tournament.PublicCode,
            Name = tournament.Name,
            Description = tournament.Description,
            Status = tournament.Status,
            MaxParticipants = tournament.MaxParticipants,
            CommunityId = tournament.CommunityId,
            CommunityName = tournament.Community?.Name ?? string.Empty,
            Region = tournament.Region,
            Province = tournament.Province,
            City = tournament.City,
            CreatedAt = tournament.CreatedAt
        };
    }

    private static ParticipantResponse MapToParticipantResponse(TournamentParticipant participant)
    {
        return new ParticipantResponse
        {
            Id = participant.Id,
            TournamentId = participant.TournamentId,
            UserId = participant.UserId,
            DisplayName = participant.DisplayName,
            TeamName = participant.TeamName,
            IsManualEntry = participant.IsManualEntry,
            IsPaid = participant.IsPaid,
            Seed = participant.Seed,
            CheckedIn = participant.CheckedIn,
            CreatedAt = participant.CreatedAt
        };
    }

    private static StageResponse MapToStageResponse(TournamentStage stage)
    {
        return new StageResponse
        {
            Id = stage.Id,
            TournamentId = stage.TournamentId,
            Name = stage.Name,
            StageOrder = stage.StageOrder,
            FormatType = stage.FormatType,
            NumberOfRounds = stage.NumberOfRounds,
            GroupCount = stage.GroupCount,
            AdvanceCount = stage.AdvanceCount,
            HasThirdPlaceMatch = stage.HasThirdPlaceMatch,
            HasGrandFinalReset = stage.HasGrandFinalReset,
            Status = stage.Status
        };
    }
}

// Made with Bob